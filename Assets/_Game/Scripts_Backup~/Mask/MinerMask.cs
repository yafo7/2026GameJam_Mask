using UnityEngine;
using UnityEngine.Tilemaps;

public class MinerMask : MaskAbility // 继承自 MaskAbility
{
    [Header("挖掘设置")]
    public Tilemap destructibleTilemap; 
    public Transform selector;          
    public TileBase dirtTile;           

    // 🎭 进入状态：修改 PlayerController 的参数
    public override void OnEnterMask()
    {
        base.OnEnterMask(); 
        
        if (player != null)
        {
            // 矿工模式下，身体变笨重
            player.moveSpeed = 4f;  
            player.jumpForce = 7f;  
        }

        if (selector != null) selector.gameObject.SetActive(true);
        Debug.Log("进入状态：矿工模式");
    }

    // 🚫 退出状态
    public override void OnExitMask()
    {
        base.OnExitMask();
        if (selector != null) selector.gameObject.SetActive(false);
    }

    // 🔄 每帧只负责更新框框位置
    void Update()
    {
        UpdateDigTarget();
    }

    // ⚔️ J键：挖
    public override void OnActionJ()
    {
        if (selector == null) return;
        Vector3Int targetGridPos = destructibleTilemap.WorldToCell(selector.position);

        if (destructibleTilemap.HasTile(targetGridPos))
        {
            destructibleTilemap.SetTile(targetGridPos, null);
            // 可以在这里播放音效：AudioManager.Play("Dig");
        }
    }

    // ⚔️ K键：填
    public override void OnActionK()
    {
        if (selector == null || dirtTile == null) return;
        
        Vector3Int targetGridPos = destructibleTilemap.WorldToCell(selector.position);
        Vector3Int playerGridPos = destructibleTilemap.WorldToCell(transform.position);

        if (!destructibleTilemap.HasTile(targetGridPos) && targetGridPos != playerGridPos)
        {
            destructibleTilemap.SetTile(targetGridPos, dirtTile);
        }
    }

    // 🎯 计算目标 (完全依赖 PlayerController 的数据)
    void UpdateDigTarget()
    {
        // 1. 获取身体的位置
        Vector3Int playerGridPos = destructibleTilemap.WorldToCell(transform.position);
        Vector3Int offset = Vector3Int.zero;

        // 2. 决定挖掘方向
        // W/S 还是得自己监听，因为 PlayerController 不管这两个键
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
            // 3. 【核心修改】直接问 PlayerController：你现在脸朝哪？
            // 这样就保证了面具和身体永远同步
            if (player.isFacingRight)
                offset = Vector3Int.right;
            else
                offset = Vector3Int.left;
        }

        Vector3Int targetGridPos = playerGridPos + offset;

        // 4. 移动框框
        if (selector != null)
        {
            selector.position = destructibleTilemap.GetCellCenterWorld(targetGridPos);
        }
    }
}