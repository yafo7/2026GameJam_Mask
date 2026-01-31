using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [Header("配置")]
    public GameObject[] stateModels; // 对应状态1-5的子物体模型(Prefab1-5)
    public Rigidbody2D rb;
    //public Tilemap groundTilemap; // 用于状态4的地图交互
    //public TileBase buildTile;    // 状态4要放置的方块

    [Header("战斗配置")]
    public GameObject arrowPrefab;
    public Transform firePoint;

    // 当前状态
    private PlayerState currentState;

    // 具体状态实例
    public StateNormal stateNormal;
    public StatePlatformer statePlatformer;
    public StateCombat stateCombat;
    public StateBuilder stateBuilder;
    // public StateIdle stateIdle; // 状态5预留

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 初始化所有状态
        stateNormal = new StateNormal(this);
        statePlatformer = new StatePlatformer(this);
        stateCombat = new StateCombat(this);
        stateBuilder = new StateBuilder(this);

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

        // 3. 切换逻辑
        switch (stateIndex)
        {
            case 1: currentState = stateNormal; break;
            case 2: currentState = statePlatformer; break;
            case 3: currentState = stateCombat; break;
            case 4: currentState = stateBuilder; break;
            default: Debug.Log("State 5 未定义"); break;
        }

        currentState.Enter();
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
}