using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("身体参数 (会被面具修改)")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public bool isFacingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // === 1. 基础移动 (所有面具通用的) ===
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // 记录朝向
        if (moveX > 0) isFacingRight = true;
        else if (moveX < 0) isFacingRight = false;

        // === 2. 基础跳跃 (通用) ===
        // 如果某个面具（比如俄罗斯方块）需要禁止跳跃，可以在那个面具的 Update 里把 rb.velocity.y 锁死
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}