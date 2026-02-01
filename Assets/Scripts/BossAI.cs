using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    [Header("属性")]
    public float maxHealth = 999f;
    public float currentHealth;
    public float moveSpeed = 2f;
    public float chaseRadius = 8f;

    [Header("UI 引用")]
    public Slider healthSlider;     // 拖入刚才创建的 Slider
    public Text healthText; // 拖入刚才创建的 Text

    private Transform player;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 初始化 UI
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        UpdateUI();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // 如果玩家进入半径，进行追踪
        if (distance < chaseRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            // 只在水平方向移动
            transform.Translate(new Vector2(direction.x, 0) * moveSpeed * Time.deltaTime);

            // 保持 UI 不随 BOSS 翻转 (可选)
            // 防止 BOSS 转身时血条文字变反
            if (healthSlider != null) healthSlider.transform.parent.localScale = new Vector3(Mathf.Sign(transform.localScale.x), 1, 1);

            // 简单的镜像翻转
            if (direction.x > 0) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
            else transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
    }

    // 处理伤害
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0); // 确保不会变成负数
        Debug.Log($"BOSS受到 {amount} 点伤害，剩余血量: {currentHealth}");

        UpdateUI();

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void UpdateUI()
    {
        if (healthSlider != null) healthSlider.value = currentHealth;
        if (healthText != null) healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    void Die()
    {
        isDead = true;
        Debug.Log("BOSS 被击败了！");
        // 这里可以播放死亡动画或弹出胜利UI
        Destroy(gameObject, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 碰到玩家，玩家掉血
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(1);
        }
    }
}
