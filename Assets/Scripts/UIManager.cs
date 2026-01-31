using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("血量UI")]
    public Image[] heartImages; // 拖入3个心形图片
    public Sprite fullHeart;    // 实心心
    public Sprite emptyHeart;   // 空心心

    [Header("状态UI")]
    public Image[] stateIcons;  // 拖入4个状态图标
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(1, 1, 1, 0.3f);

    [Header("游戏结束")]
    public GameObject gameOverPanel;

    void Awake() { Instance = this; }

    // 更新血量显示
    public void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < currentHealth ? fullHeart : emptyHeart;
        }

        if (currentHealth <= 0) gameOverPanel.SetActive(true);
    }

    // 更新状态高亮
    /*public void UpdateStateUI(int activeIndex)
    {
        for (int i = 0; i < stateIcons.Length; i++)
        {
            stateIcons[i].color = (i == activeIndex - 1) ? activeColor : inactiveColor;
        }
    }*/
}
