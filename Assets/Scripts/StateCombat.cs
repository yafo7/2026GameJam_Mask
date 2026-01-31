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
        var combatScript = player.stateModels[2].GetComponentInChildren<MeleeHandler>();
        if (combatScript) combatScript.Attack();
    }

    void RangeAttack()
    {
        if (player.arrowPrefab != null && player.firePoint != null)
        {
            // 1. 生成箭矢
            GameObject arrowObj = Object.Instantiate(player.arrowPrefab, player.firePoint.position, Quaternion.identity);

            // 2. 获取箭矢脚本
            ArrowProjectile arrowScript = arrowObj.GetComponent<ArrowProjectile>();

            // 【关键检查点】你有没有写下面这一段？
            if (arrowScript != null)
            {
                // 获取朝向：玩家缩放X是正就是1，是负就是-1
                float direction = Mathf.Sign(player.transform.localScale.x);

                // 【必须调用】如果不调用这句，ArrowProjectile里的代码一行都不会执行！
                arrowScript.Setup(direction);
            }
            else
            {
                Debug.LogError("生成的箭矢上没有挂载 ArrowProjectile 脚本！");
            }
        }
    }
}