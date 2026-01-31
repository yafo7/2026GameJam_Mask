using UnityEngine;

//马里奥
public class StatePlatformer : PlayerState
{
    private float jumpForce = 10f;

    public StatePlatformer(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("进入状态2：跳跃模式");
    }

    public override void HandleInput()
    {
        base.HandleInput(); // 保持左右移动

        if (Input.GetKeyDown(KeyCode.W))
        {
            // 简单的落地检测逻辑略（建议使用Raycast）
            // 这里直接给向上的速度
            player.SetVelocityY(jumpForce);
        }
    }
}