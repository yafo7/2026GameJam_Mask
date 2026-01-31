using UnityEngine;


//林克
public class StateCombat : PlayerState
{
    private float attackRate = 0.5f;
    private float nextAttackTime = 0;

    public StateCombat(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("进入状态3：战斗模式");
    }

    public override void HandleInput()
    {
        base.HandleInput();

        player.PerformMovement();

        // 状态3增加跳跃
        if (Input.GetKeyDown(KeyCode.W)) player.PerformJump();

        // 挥剑 (J)
        if (Input.GetKeyDown(KeyCode.J) && Time.time >= nextAttackTime)
        {
            MeleeAttack();
            nextAttackTime = Time.time + attackRate;
        }

        // 射箭 (K)
        if (Input.GetKeyDown(KeyCode.K) && Time.time >= nextAttackTime)
        {
            RangeAttack();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void MeleeAttack()
    {
        // 这里的逻辑通常是触发Animator的Trigger，然后在动画特定帧开启碰撞盒
        // 为简化，假设我们直接调用Prefab3上的攻击脚本
        /*var combatScript = player.stateModels[2].GetComponentInChildren<MeleeHandler>();
        if (combatScript) combatScript.Attack();*/
        Debug.Log("挥剑攻击！");
        // 1. 在攻击点画一个圆，获取圆内所有的碰撞体
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(player.firePoint.position, player.attackRange);

        // 2. 遍历这些物体
        foreach (Collider2D obj in hitObjects)
        {
            // 碰到怪物
            if (obj.CompareTag("Enemy"))
            {
                Debug.Log("击中怪物，销毁！");
                Object.Destroy(obj.gameObject);
            }
            // 碰到草丛 (假设草丛的 Tag 是 "Grass")
            else if (obj.CompareTag("Grass"))
            {
                Debug.Log("斩断草丛！");
                Object.Destroy(obj.gameObject);
            }
        }

    }

    void RangeAttack()
    {
        if (player.arrowPrefab != null && player.firePoint != null)
        {
            // 1. 生成箭矢
            GameObject arrowObj = Object.Instantiate(player.arrowPrefab, player.firePoint.position, Quaternion.identity);

            // 2. 获取箭矢脚本
            ArrowProjectile arrowScript = arrowObj.GetComponent<ArrowProjectile>();

            if (arrowScript != null)
            {
                // 获取朝向：玩家缩放X是正就是1，是负就是-1
                float direction = Mathf.Sign(player.transform.localScale.x);

                //如果不调用这句，ArrowProjectile里的代码一行都不会执行！
                arrowScript.Setup(direction);
            }
            else
            {
                Debug.LogError("生成的箭矢上没有挂载 ArrowProjectile 脚本！");
            }
        }
    }
}