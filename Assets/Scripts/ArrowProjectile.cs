using UnityEngine;


//林克射箭的箭
public class ArrowProjectile : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;

    void Start()
    {
        rb.velocity = transform.right * transform.localScale.x * speed; // 根据朝向飞行
        Destroy(gameObject, 3f); // 3秒后自动销毁
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // 杀死怪物
            Destroy(gameObject); // 销毁箭
        }
    }
}
