using UnityEngine;

// 这是一个抽象类，不能直接挂载，只能被继承
public abstract class MaskAbility : MonoBehaviour
{
    protected PlayerController player; // 引用主角控制器，用于修改移动参数

    protected virtual void Awake()
    {
        player = GetComponent<PlayerController>();
        // 默认把自己关掉，等待 Manager 唤醒
        enabled = false; 
    }

    // 🎭 当切换到这个面具时触发 (初始化数据，比如改跳跃力、换UI)
    public virtual void OnEnterMask()
    {
        this.enabled = true; // 开启 Update 循环
        Debug.Log($"切换到了面具: {this.GetType().Name}");
    }

    // 🚫 当切换走时触发 (清理数据，比如取消无敌、恢复重力)
    public virtual void OnExitMask()
    {
        this.enabled = false; // 关闭 Update 循环
    }

    // ⚔️ 核心技能接口 (由 Manager 调用)
    public abstract void OnActionJ(); // J键逻辑
    public abstract void OnActionK(); // K键逻辑
}