using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [Header("配置")]
    public GameObject[] stateModels; // 对应状态1-5的子物体模型(Prefab1-5)
    public Rigidbody2D rb;

    [Header("状态4：建造模式配置")]
    public Tilemap groundTilemap; // 地形Tilemap
    public TileBase buildTile;    // 要建造的方块资源
    public Transform gridSelector; // 【新增】可视化选框（一个半透明的方块）

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
    public Transform keyHoldPoint; // 钥匙挂载点（在玩家前方）
    private GameObject carriedKey; // 当前携带的钥匙

    //用来存储在编辑器里设置的缩放大小
    public Vector3 initialScale;

    // 当前状态
    private PlayerState currentState;

    // 具体状态实例
    public StateNormal stateNormal;
    public StatePlatformer statePlatformer;
    public StateCombat stateCombat;
    public StateBuilder stateBuilder;
    public StateGunner stateGunner; // 状态5实例

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //游戏一开始，先记录下当前的形状大小
        initialScale = transform.localScale;

        // 初始化所有状态
        stateNormal = new StateNormal(this);
        statePlatformer = new StatePlatformer(this);
        stateCombat = new StateCombat(this);
        stateBuilder = new StateBuilder(this);
        stateGunner = new StateGunner(this);

        // 默认关闭选框（只有切到状态4才打开）
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
    }

    void FixedUpdate()
    {
        currentState?.PhysicsUpdate();
    }

    // 状态切换核心逻辑
    public void SwitchState(int stateIndex)
    {
        if (currentState != null) currentState.Exit();

        // 1. 隐藏所有模型
        foreach (var model in stateModels) model.SetActive(false);

        // 2. 激活对应模型 (索引从0开始，所以减1)
        if (stateIndex - 1 < stateModels.Length)
            stateModels[stateIndex - 1].SetActive(true);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateStateUI(stateIndex); // 传入 1, 2, 3, 4, 5
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
        UIManager.Instance.UpdateHealthUI(health);
        if (health <= 0)
        {
            rb.simulated = false; // 禁用物理
        }
    }

    // 加血逻辑
    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        UIManager.Instance.UpdateHealthUI(health);
    }

    // 拾取钥匙逻辑
    public void PickUpKey(GameObject key)
    {
        if (carriedKey != null) return;
        carriedKey = key;
        key.transform.SetParent(this.transform);
        // 实时更新位置到玩家前方 (FlipCharacter时 keyHoldPoint 会跟着转)
        key.transform.localPosition = new Vector3(1f, 0, 0);
    }

    // --- 通用动作方法 ---
    public void PerformMovement(float speedMultiplier = 1f)
    {
        float h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed * speedMultiplier, rb.velocity.y);
        if (Mathf.Abs(h) > 0.01f)
        {
            float dir = Mathf.Sign(h);
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x) * dir, initialScale.y, initialScale.z);
        }
    }

    public void PerformJump(float forceMultiplier = 1f)
    {
        // 简单的地面判定：纵向速度接近0时可跳
        if (Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * forceMultiplier);
        }
    }

    // --- 公共辅助方法 ---
    public void SetVelocityX(float x)
    {
        rb.velocity = new Vector2(x, rb.velocity.y);
    }

    public void SetVelocityY(float y)
    {
        rb.velocity = new Vector2(rb.velocity.x, y);
    }

    // ---通用的翻转方法 ---
    // 所有的状态(State)都应该调用这个方法
    public void FlipCharacter(float xInput)
    {
        if (Mathf.Abs(xInput) > 0.01f) // 只有当有输入时才判断
        {
            // 获取方向：1 是右，-1 是左
            float direction = Mathf.Sign(xInput);

            // 使用 initialScale (初始大小) 的绝对值，乘以方向
            // 这样无论你怎么缩放，它都会保持那个大小，只是改变朝向
            transform.localScale = new Vector3(
                Mathf.Abs(initialScale.x) * direction,
                initialScale.y,
                initialScale.z
            );
        }
    }
}