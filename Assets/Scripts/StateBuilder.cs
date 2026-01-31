using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class StateBuilder : PlayerState
{
    // 构造函数
    public StateBuilder(PlayerController player) : base(player) { }

    // 🎭 进入状态：激活选框
    public override void Enter()
    {
        Debug.Log("进入状态4：建造模式");
        if (player.gridSelector != null)
        {
            player.gridSelector.gameObject.SetActive(true);
            // 确保进入瞬间立即更新一次位置，防止选框闪烁
            UpdateSelectorPosition();
        }
    }

    // 🚫 退出状态：隐藏选框
    public override void Exit()
    {
        if (player.gridSelector != null)
            player.gridSelector.gameObject.SetActive(false);
    }

    public override void HandleInput()
    {
        // 1. 基础水平移动
        float h = Input.GetAxisRaw("Horizontal");
        player.SetVelocityX(h * player.moveSpeed * 0.8f); // 建造时移动稍微慢一点

        // 调用通用的翻转逻辑（确保 Scale.x 正确）
        player.FlipCharacter(h);

        // 2. 核心：更新选框位置 (同步 MinerMask 逻辑)
        UpdateSelectorPosition();

        // 3. 跳跃功能
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.PerformJump();
        }

        // 4. 监听 J 键 (破坏)
        if (Input.GetKeyDown(KeyCode.J))
        {
            PerformDestroy();
        }

        // 5. 监听 K 键 (建造)
        if (Input.GetKeyDown(KeyCode.K))
        {
            PerformBuild();
        }
    }

    // 🎯 计算目标位置 (完全同步自 MinerMask.UpdateDigTarget)
    void UpdateSelectorPosition()
    {
        if (player.gridSelector == null || player.groundTilemap == null) return;

        // 获取玩家所在的格子坐标
        Vector3Int playerGridPos = player.groundTilemap.WorldToCell(player.transform.position);
        Vector3Int offset = Vector3Int.zero;

        // 决定偏移方向：W/S 优先，否则根据当前 Scale 判断左右
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
            // 使用 Mathf.Sign 确保即使缩放不是 1 也能正确判断正负
            if (player.transform.localScale.x > 0)
                offset = Vector3Int.right;
            else
                offset = Vector3Int.left;
        }

        Vector3Int targetGridPos = playerGridPos + offset;

        // 移动框框到目标格子的中心
        //player.gridSelector.position = player.groundTilemap.GetCellCenterWorld(targetGridPos);

        Vector3 cellCenter = player.groundTilemap.GetCellCenterWorld(targetGridPos);
        // 将 Z 设为 -1f (确保在 Z=0 的 Tilemap 前面)
        player.gridSelector.position = new Vector3(cellCenter.x, cellCenter.y, -1f);

    }

    // ⚔️ 破坏逻辑
    void PerformDestroy()
    {
        if (player.gridSelector == null) return;

        Vector3Int targetPos = player.groundTilemap.WorldToCell(player.gridSelector.position);

        if (player.groundTilemap.HasTile(targetPos))
        {
            player.groundTilemap.SetTile(targetPos, null);
            Debug.Log($"破坏了位置 {targetPos} 的方块");
        }
    }

    // ⚔️ 建造逻辑
    void PerformBuild()
    {
        if (player.gridSelector == null || player.buildTile == null) return;

        Vector3Int targetPos = player.groundTilemap.WorldToCell(player.gridSelector.position);
        Vector3Int playerPos = player.groundTilemap.WorldToCell(player.transform.position);

        // 防止把自己埋在墙里：如果目标位置就是玩家站的位置，不允许建造
        if (targetPos == playerPos) return;

        // 如果该位置是空的，就填上方块
        if (player.groundTilemap.GetTile(targetPos) == null)
        {
            player.groundTilemap.SetTile(targetPos, player.buildTile);
            Debug.Log($"在位置 {targetPos} 放置了方块");
        }
    }
}