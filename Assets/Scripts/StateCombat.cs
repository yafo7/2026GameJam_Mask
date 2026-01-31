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
        // 实例化箭矢
        if (player.arrowPrefab && player.firePoint)
        {
            GameObject arrow = Object.Instantiate(player.arrowPrefab, player.firePoint.position, player.transform.rotation);
            // 箭矢脚本会自动处理飞行
        }
    }
}