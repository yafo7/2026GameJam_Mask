using UnityEngine;

public class MarioMask : MaskAbility // 继承自 MaskAbility
{
    [Header("马里奥参数")]
    public float marioJumpForce = 12f; // 马里奥跳得很高
    public float marioMoveSpeed = 6f;  // 马里奥跑得也快一点

    // 🎭 进入状态：修改身体参数
    public override void OnEnterMask()
    {
        base.OnEnterMask();
        
        if (player != null)
        {
            player.jumpForce = marioJumpForce;
            player.moveSpeed = marioMoveSpeed;
        }
        
        Debug.Log("进入状态：马里奥模式 (跳跃力 UP!)");
    }

    // 🚫 退出状态
    public override void OnExitMask()
    {
        base.OnExitMask();
        // 不需要特意重置，因为下一个面具（比如矿工）会在它自己的 OnEnterMask 里把数值改回去
    }

    // ⚔️ 技能键 (目前马里奥没有特殊技能，或者是钻水管)
    public override void OnActionJ()
    {
        Debug.Log("马里奥摸了摸胡子 (J键暂无功能)");
    }

    public override void OnActionK()
    {
        Debug.Log("马里奥整理了帽子 (K键暂无功能)");
    }
}