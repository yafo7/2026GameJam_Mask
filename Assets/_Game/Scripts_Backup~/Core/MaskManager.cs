using UnityEngine;
using System.Collections.Generic;

public class MaskManager : MonoBehaviour
{
    [Header("配置所有面具")]
    public MaskAbility marioMask;
    public MaskAbility zeldaMask;
    public MaskAbility minerMask;
    public MaskAbility tetrisMask;
    public MaskAbility pokemonMask;
    public MaskAbility p5Mask;

    // 当前正在使用的面具
    private MaskAbility currentMask;

    void Start()
    {
        // 游戏开始，默认激活 Mario 或者 Miner
        SwitchMask(minerMask); 
    }

    void Update()
    {
        // === 1. 监听切换输入 ===
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchMask(marioMask);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchMask(zeldaMask);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchMask(minerMask);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchMask(tetrisMask);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SwitchMask(pokemonMask);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SwitchMask(p5Mask); // 如果有

        // === 2. 监听技能输入 (分发给当前面具) ===
        if (currentMask != null)
        {
            if (Input.GetKeyDown(KeyCode.J)) currentMask.OnActionJ();
            if (Input.GetKeyDown(KeyCode.K)) currentMask.OnActionK();
        }
    }

    void SwitchMask(MaskAbility newMask)
    {
        if (newMask == null || newMask == currentMask) return;

        // 1. 退出旧面具
        if (currentMask != null)
        {
            currentMask.OnExitMask();
        }

        // 2. 激活新面具
        currentMask = newMask;
        currentMask.OnEnterMask();
    }
}