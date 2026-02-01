/*using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 1; // 默认威力为1

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 尝试获取 BOSS 脚本
        BossAI boss = other.GetComponent<BossAI>();
        if (boss != null)
        {
            boss.TakeDamage(damageAmount);

            // 如果是远程消耗品（子弹或箭），碰撞后销毁自己
            if (gameObject.CompareTag("Projectile"))
            {
                Destroy(gameObject);
            }
        }
    }
}*/


using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 尝试攻击 BOSS 或 敌人
        BossAI boss = other.GetComponent<BossAI>();

        // 检查是否是敌人 (通过 Tag 或者 组件)
        bool isEnemy = other.CompareTag("Enemy") || boss != null;

        if (isEnemy)
        {
            // 如果有 Boss 组件，扣血
            if (boss != null) boss.TakeDamage(damageAmount);
            // 如果是普通敌人，直接销毁 (根据之前的逻辑)
            else Destroy(other.gameObject);

            // --- 【新增】播放“攻击命中”音效 ---
            // 只有当这个物体是玩家发出的攻击(Projectile或Sword)才播放，避免误判
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyHitClip);
            }

            // 销毁飞行道具（如果是子弹）
            if (gameObject.CompareTag("Projectile"))
                Destroy(gameObject);
        }
    }
}