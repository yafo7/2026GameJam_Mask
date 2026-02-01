using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("跟随目标")]
    public Transform target; // 拖入你的 Player

    [Header("平滑参数 (0-1)")]
    [Range(0, 1)]
    public float smoothSpeed = 0.125f; // 数值越小越慢(有延迟感)，越大越快

    [Header("位置偏移")]
    public Vector3 offset; // 建议 Z 设为 -10，否则相机会切进地里

    void LateUpdate() // 使用 LateUpdate 确保在主角移动完之后再跟，防止抖动
    {
        if (target == null) return;

        // 1. 计算目标位置
        Vector3 desiredPosition = target.position + offset;

        // 2. 平滑移动 (Lerp插值)
        // 从 "当前位置" 慢慢移动到 "目标位置"
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 3. 应用位置
        transform.position = smoothedPosition;
    }
}