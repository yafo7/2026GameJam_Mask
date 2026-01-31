using UnityEngine;

// 抽象基类，定义状态的通用接口
public abstract class PlayerState
{
    protected PlayerController player;
    protected float moveSpeed = 5f;

    public PlayerState(PlayerController _player)
    {
        player = _player;
    }

    public virtual void Enter() { } // 进入状态时调用
    public virtual void HandleInput() // 处理按键逻辑
    {
        // 通用的左右移动逻辑
        float h = Input.GetAxisRaw("Horizontal"); // A/D
        player.SetVelocityX(h * moveSpeed);

        /*if (h != 0)
        {
            // 处理朝向翻转
            player.transform.localScale = new Vector3(h > 0 ? 1 : -1, 1, 1);
        }*/

        player.FlipCharacter(h);
    }
    public virtual void PhysicsUpdate() { } // 物理更新
    public virtual void Exit() { } // 退出状态时调用
}