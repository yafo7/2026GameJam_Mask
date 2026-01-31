using UnityEngine;
using UnityEngine.Tilemaps;

public class StateBuilder : PlayerState
{
    // 构造函数
    public StateBuilder(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("进入状态4：建造模式");
        // 打开可视化选框
        if (player.gridSelector != null)
            player.gridSelector.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        // 关闭可视化选框
        if (player.gridSelector != null)
            player.gridSelector.gameObject.SetActive(false);
    }

    public override void HandleInput()
    {
        // 1. 保持移动能力 (继承基类通用逻辑)
        base.HandleInput();

        // 2. 更新选框位置 (移植自 MinerMask.UpdateDigTarget)
        UpdateSelectorPosition();

        player.PerformMovement(0.8f); // 建造时移动稍微慢一点

        // 状态4增加跳跃功能
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.PerformJump();
        }

        // 3. 监听 J 键 (破坏)
        if (Input.GetKeyDown(KeyCode.J))
        {
            PerformDestroy();
        }

        // 4. 监听 K 键 (建造)
        if (Input.GetKeyDown(KeyCode.K))
        {
            PerformBuild();
        }
    }

    // --- 核心辅助逻辑 ---

    void UpdateSelectorPosition()
    {
        if (player.gridSelector == null || player.groundTilemap == null) return;

        // 获取玩家所在的格子坐标
        Vector3Int playerGridPos = player.groundTilemap.WorldToCell(player.transform.position);
        Vector3Int offset = Vector3Int.zero;

        // 逻辑：W优先上，S优先下，否则根据玩家朝向决定左右
        if (Input.GetKey(KeyCode.W))
        {
            offset = Vector3Int.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            offset = Vector3Int.down;
        }
        else
        {
            // 根据玩家 Scale.x 判断朝向 (1为右, -1为左)
            if (player.transform.localScale.x > 0)
                offset = Vector3Int.right;
            else
                offset = Vector3Int.left;
        }

        // 计算目标格子的世界坐标
        Vector3Int targetGridPos = playerGridPos + offset;

        // 更新选框物体的位置（对齐到网格中心）
        player.gridSelector.position = player.groundTilemap.GetCellCenterWorld(targetGridPos);
    }

    void PerformDestroy()
    {
        if (player.gridSelector == null) return;

        // 获取选框当前所在的格子
        Vector3Int targetPos = player.groundTilemap.WorldToCell(player.gridSelector.position);

        // 如果有东西，就挖掉 (设为 null)
        if (player.groundTilemap.HasTile(targetPos))
        {
            player.groundTilemap.SetTile(targetPos, null);
            Debug.Log($"破坏了位置 {targetPos} 的方块");
        }
    }

    void PerformBuild()
    {
        if (player.gridSelector == null || player.buildTile == null) return;

        Vector3Int targetPos = player.groundTilemap.WorldToCell(player.gridSelector.position);
        Vector3Int playerPos = player.groundTilemap.WorldToCell(player.transform.position);

        // 防止把自己埋在墙里：如果目标位置就是玩家站的位置，不允许建造
        if (targetPos == playerPos) return;

        // 如果该位置是空的，就填上土块
        if (player.groundTilemap.GetTile(targetPos) == null)
        {
            player.groundTilemap.SetTile(targetPos, player.buildTile);
            Debug.Log($"在位置 {targetPos} 放置了方块");
        }
    }
}
