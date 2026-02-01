using UnityEngine;

public class StateGunner : PlayerState
{
    public StateGunner(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("进入状态5：枪手模式");
    }

    public override void HandleInput()
    {
        // 1. 左右移动 (A/D)
        float h = Input.GetAxisRaw("Horizontal");
        player.SetVelocityX(h * player.moveSpeed);
        player.FlipCharacter(h);

        // 2. 跳跃 (复用之前的通用跳跃)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.PerformJump();
        }

        // 3. 开枪逻辑 (J键)
        if (Input.GetKeyDown(KeyCode.J))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (player.bulletPrefab == null || player.firePoint == null) return;

        // 生成子弹
        GameObject bullet = Object.Instantiate(player.bulletPrefab, player.firePoint.position, Quaternion.identity);

        // 给子弹设置高额伤害 (333)
        DamageDealer dealer = bullet.AddComponent<DamageDealer>();
        dealer.damageAmount = 333;
        bullet.tag = "Projectile"; // 标记为飞行物

        // 计算方向：根据玩家当前的缩放正负来决定子弹朝向
        float direction = Mathf.Sign(player.transform.localScale.x);

        // 假设你的子弹Prefab自带移动脚本，或者在这里给它一个速度
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(direction * 15f, 0); // 15f 是子弹速度
        }

        // 旋转子弹图片（如果是左向）
        if (direction < 0)
        {
            bullet.transform.localScale = new Vector3(-1, 1, 1);
        }

        // 3秒后自动销毁子弹防止卡顿
        Object.Destroy(bullet, 3f);
    }
}