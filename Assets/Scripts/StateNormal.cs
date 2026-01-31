using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//初始
public class StateNormal : PlayerState
{
    public StateNormal(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("进入状态1：普通模式");
        moveSpeed = 5f; // 可以设定特定速度
    }

    // 只继承基类的 HandleInput (A/D移动)，无特殊功能
}