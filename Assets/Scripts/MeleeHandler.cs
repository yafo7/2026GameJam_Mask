using UnityEngine;
using System.Collections;

//林克的剑
public class MeleeHandler : MonoBehaviour
{
    public GameObject swordCollider; // 拖入挂了BodyPartTrigger(Sword)的物体

    public void Attack()
    {
        StartCoroutine(EnableHitbox());
    }

    IEnumerator EnableHitbox()
    {
        swordCollider.SetActive(true);
        // 假设挥剑动画持续0.2秒
        yield return new WaitForSeconds(0.2f);
        swordCollider.SetActive(false);
    }
}
