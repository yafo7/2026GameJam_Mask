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
    public Image[] stateIcons;  // 拖入5个状态图标
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // 未选中时的颜色（半透明变暗）

    [Header("游戏结束")]
    public GameObject gameOverPanel;

    void Awake() { Instance = this; }

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // 关掉它！
        }
    }

    // 更新血量显示
    public void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < currentHealth ? fullHeart : emptyHeart;
        }

        if (currentHealth <= 0) gameOverPanel.SetActive(true);
    }

    public void UpdateStateUI(int activeIndex)
    {
        // activeIndex 传进来是 1~5，但数组下标是 0~4
        for (int i = 0; i < stateIcons.Length; i++)
        {
            if (i == activeIndex - 1)
            {
                // 选中的图标：设为高亮色，甚至可以稍微放大一点增加反馈
                stateIcons[i].color = activeColor;
                stateIcons[i].transform.localScale = Vector3.one * 1.2f; // (可选) 放大1.2倍
            }
            else
            {
                // 未选中的图标：设为暗色，恢复原大小
                stateIcons[i].color = inactiveColor;
                stateIcons[i].transform.localScale = Vector3.one;
            }
        }
    }
}
