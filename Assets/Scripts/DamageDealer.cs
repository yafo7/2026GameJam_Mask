using UnityEngine;

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
}