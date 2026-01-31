using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public enum ItemType { Grass, Heart, Key }
    public ItemType type;

    // 爱心和钥匙通过碰撞触发
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (type == ItemType.Heart)
            {
                pc.Heal(1);
                Destroy(gameObject);
            }
            else if (type == ItemType.Key)
            {
                pc.PickUpKey(this.gameObject);
                // 禁用掉原本的触发器，防止重复触发
                GetComponent<Collider2D>().isTrigger = false;
            }
        }
    }

    // 草丛被挥剑斩断 (在 StateCombat 中检测)
    public void OnHitBySword()
    {
        if (type == ItemType.Grass)
        {
            Debug.Log("草丛被斩断");
            Destroy(gameObject);
        }
    }
}
