using UnityEngine;

public class BodyPartTrigger : MonoBehaviour
{
    public enum PartType { Head, Feet, Sword }
    public PartType partType;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 马里奥逻辑：脚踩怪物
        if (partType == PartType.Feet && collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            // 给玩家一个反弹跳力
            GetComponentInParent<Rigidbody2D>().velocity = Vector2.up * 5f;
        }

        // 马里奥逻辑：头顶砖块
        if (partType == PartType.Head && collision.CompareTag("BreakableBrick"))
        {
            Destroy(collision.gameObject);
        }

        // 林克逻辑：剑砍
        if (partType == PartType.Sword)
        {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Grass"))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}