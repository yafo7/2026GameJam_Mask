using UnityEngine;
using UnityEngine.SceneManagement; // 必须引用场景管理命名空间

public class MainMenu : MonoBehaviour
{
    // 点击“开始游戏”时调用
    public void StartGame()
    {
        // 跳转到名为 "Game" 的场景
        // 请确保你在 Build Settings 中已经添加了该场景
        SceneManager.LoadScene("TestScene");
    }

    // 点击“退出”时调用
    public void QuitGame()
    {
        Debug.Log("游戏已退出"); // 在编辑器测试时会看到这条日志

        // 实际打包后的退出逻辑
        Application.Quit();
    }
}