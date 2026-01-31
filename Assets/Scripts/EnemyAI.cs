using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float patrolRange = 3f;
    public float chaseRange = 5f;
    public float moveSpeed = 2f;

    private Vector3 startPos;
    private bool chasing = false;
    private Transform playerTrans;

    void Start()
    {
        startPos = transform.position;
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, playerTrans.position);

        if (distToPlayer < chaseRange)
        {
            // 追逐玩家
            Vector2 dir = (playerTrans.position - transform.position).normalized;
            transform.Translate(new Vector2(dir.x, 0) * moveSpeed * Time.deltaTime);
        }
        else
        {
            // 在起始点附近来回移动 (使用 Mathf.PingPong)
            float x = Mathf.PingPong(Time.time * moveSpeed, patrolRange * 2) - patrolRange;
            transform.position = new Vector3(startPos.x + x, transform.position.y, transform.position.z);
        }
    }

    // 碰撞玩家
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(1);
        }
    }
}