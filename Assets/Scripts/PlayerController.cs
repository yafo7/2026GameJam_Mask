using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [Header("配置")]
    public GameObject[] stateModels; // 对应状态1-5的子物体模型(Prefab1-5)
    public Rigidbody2D rb;

    // 【新增】核心变量：用来记住当前正在使用的那个动画机
    private Animator currentAnimator;

    [Header("状态4：建造模式配置")]
    public Tilemap groundTilemap; // 地形Tilemap
    public TileBase buildTile;    // 要建造的方块资源
    public Transform gridSelector; // 可视化选框

    [Header("战斗配置")]
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float attackRange = 1.2f; // 挥剑检测半径

    [Header("状态5：枪手配置")]
    public GameObject bulletPrefab; // 状态5专用的子弹Prefab

    [Header("跳跃配置")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("玩家属性")]
    public int health = 3;
    public int maxHealth = 3;
    public Transform keyHoldPoint; // 钥匙挂载点
    private GameObject carriedKey; // 当前携带的钥匙

    // 用来存储在编辑器里设置的缩放大小
    public Vector3 initialScale;

    // 当前状态
    private PlayerState currentState;

    // 具体状态实例
    public StateNormal stateNormal;
    public StatePlatformer statePlatformer;
    public StateCombat stateCombat;
    public StateBuilder stateBuilder;
    public StateGunner stateGunner; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 游戏一开始，先记录下当前的形状大小
        initialScale = transform.localScale;

        // 初始化所有状态
        stateNormal = new StateNormal(this);
        statePlatformer = new StatePlatformer(this);
        stateCombat = new StateCombat(this);
        stateBuilder = new StateBuilder(this);
        stateGunner = new StateGunner(this);

        // 默认关闭选框
        if (gridSelector != null) gridSelector.gameObject.SetActive(false);

        // 默认进入状态1
        SwitchState(1);
    }

    void Update()
    {
        // 全局按键监听状态切换
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchState(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchState(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchState(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchState(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SwitchState(5);

        // 执行当前状态的逻辑
        currentState?.HandleInput();

        // 【新增】每一帧都去更新动画
        // 这一步如果不加，Animator 永远收不到 Speed 参数！
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        currentState?.PhysicsUpdate();
    }

    // 【新增】专门处理动画参数的方法
    void UpdateAnimation()
    {
        // 只有当获取到了动画机，才去设置参数，防止报错
        if (currentAnimator != null)
        {
            // 获取刚体X轴速度的绝对值（因为向左走速度是负数，但我们要传正数）
            float speed = Mathf.Abs(rb.velocity.x);
            
            // 传给 Animator 里的 "Speed" 参数
            // 确保你在 Unity Animator 面板里设的参数名也是 "Speed"
            currentAnimator.SetFloat("Speed", speed);
        }
    }

    // 状态切换核心逻辑
    public void SwitchState(int stateIndex)
    {
        if (currentState != null) currentState.Exit();

        // 1. 隐藏所有模型
        foreach (var model in stateModels) model.SetActive(false);

        // 2. 激活对应模型 (索引从0开始，所以减1)
        if (stateIndex - 1 < stateModels.Length)
        {
            // 拿到当前要激活的模型
            GameObject activeModel = stateModels[stateIndex - 1];
            activeModel.SetActive(true);

            // 【新增】关键一步：每次切换物体，都要把它的 Animator 抓出来赋值给 currentAnimator
            // 这样 UpdateAnimation() 才能控制正确的那个物体
            currentAnimator = activeModel.GetComponent<Animator>();
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateStateUI(stateIndex); 
        }

        //音效
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayStateSwitchSound(stateIndex);
        }

        // 3. 切换逻辑
        switch (stateIndex)
        {
            case 1: currentState = stateNormal; break;
            case 2: currentState = statePlatformer; break;
            case 3: currentState = stateCombat; break;
            case 4: currentState = stateBuilder; break;
            case 5: currentState = stateGunner; break;
            default: Debug.Log("State 5 未定义"); break;
        }

        currentState.Enter();
    }

    // 受伤逻辑
    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        // 记得在这里调用你的 UIManager 更新血量
        if(UIManager.Instance != null) UIManager.Instance.UpdateHealthUI(health);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.playerHitClip);
        }

        if (health <= 0)
        {
            rb.simulated = false; // 禁用物理
            // 如果有 GameUI，可以在这里调用 Game Over
        }
    }

    // 加血逻辑
    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        if(UIManager.Instance != null) UIManager.Instance.UpdateHealthUI(health);
    }

    // 拾取钥匙逻辑
    public void PickUpKey(GameObject key)
    {
        if (carriedKey != null) return;
        carriedKey = key;
        key.transform.SetParent(this.transform);
        key.transform.localPosition = new Vector3(1f, 0, 0);
    }

    // --- 通用动作方法 ---
    public void PerformMovement(float speedMultiplier = 1f)
    {
        float h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed * speedMultiplier, rb.velocity.y);
        
        // 【建议】把翻转逻辑放在这里调用
        FlipCharacter(h);
    }

    public void PerformJump(float forceMultiplier = 1f)
    {
        if (Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * forceMultiplier);
        }
    }

    public void SetVelocityX(float x)
    {
        rb.velocity = new Vector2(x, rb.velocity.y);
    }

    public void SetVelocityY(float y)
    {
        rb.velocity = new Vector2(rb.velocity.x, y);
    }

    // ---通用的翻转方法 ---
    public void FlipCharacter(float xInput)
    {
        if (Mathf.Abs(xInput) > 0.01f) 
        {
            float direction = Mathf.Sign(xInput);

            transform.localScale = new Vector3(
                Mathf.Abs(initialScale.x) * direction,
                initialScale.y,
                initialScale.z
            );
        }
    }
}