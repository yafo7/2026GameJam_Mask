using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // 强制要求物体必须有刚体组件
public class ArrowProjectile : MonoBehaviour
{
    public float speed = 12f;
    public float lifeTime = 3f;
    private Rigidbody2D rb;

    // 注意：不要依赖Awake，直接在Setup里获取，确保万无一失
    public void Setup(float direction)
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("箭矢上找不到 Rigidbody2D 组件！");
            return;
        }

        // 核心修改：确保刚体是 Dynamic 且不受重力影响，防止配置错误
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;

        // 打印日志，调试看是否收到了方向（按F12看Console）
        Debug.Log($"箭矢发射! 方向: {direction}, 速度: {speed * direction}");

        // 设置速度
        rb.velocity = new Vector2(speed * direction, 0);

        // 处理图片翻转
        Vector3 localScale = transform.localScale;
        // 确保使用绝对值乘以方向，防止负负得正导致的翻转错误
        localScale.x = Mathf.Abs(localScale.x) * (direction > 0 ? 1 : -1);
        transform.localScale = localScale;

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground")) // 假设墙壁Tag是Ground
        {
            Destroy(gameObject);
        }
    }
}
