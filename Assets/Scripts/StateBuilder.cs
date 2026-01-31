using UnityEngine;
using UnityEngine.Tilemaps;

//史蒂夫
public class StateBuilder : PlayerState
{
    public StateBuilder(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("进入状态4：建造模式");
    }

    /*public override void HandleInput()
    {
        base.HandleInput();

        // 获取方向向量 (0,1) (0,-1) (-1,0) (1,0)
        Vector3Int dir = Vector3Int.zero;
        if (Input.GetKey(KeyCode.W)) dir = Vector3Int.up;
        else if (Input.GetKey(KeyCode.S)) dir = Vector3Int.down;
        else if (Input.GetKey(KeyCode.A)) dir = Vector3Int.left;
        else if (Input.GetKey(KeyCode.D)) dir = Vector3Int.right;

        // 必须按住方向键才能操作
        if (dir != Vector3Int.zero)
        {
            // 玩家当前所在的格子坐标
            Vector3Int playerPos = player.groundTilemap.WorldToCell(player.transform.position);
            Vector3Int targetPos = playerPos + dir;

            // J 键破坏
            if (Input.GetKeyDown(KeyCode.J))
            {
                player.groundTilemap.SetTile(targetPos, null); // 设置为null即为删除
            }

            // K 键放置
            if (Input.GetKeyDown(KeyCode.K))
            {
                // 检测该位置是否为空，避免卡住自己
                if (player.groundTilemap.GetTile(targetPos) == null)
                {
                    player.groundTilemap.SetTile(targetPos, player.buildTile);
                }
            }
        }
    }*/
}
