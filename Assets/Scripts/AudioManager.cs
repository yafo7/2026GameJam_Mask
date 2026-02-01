using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("音频源组件")]
    public AudioSource musicSource; // 用来播放背景音乐 (Loop)
    public AudioSource sfxSource;   // 用来播放短音效 (OneShot)

    [Header("音乐资源")]
    public AudioClip backgroundMusic;

    [Header("音效资源")]
    // 数组索引对应状态：0空置, 1=普通, 2=跳跃者, 3=战斗, 4=建造, 5=枪手
    public AudioClip[] stateSwitchClips;

    public AudioClip playerHitClip; // 玩家受伤
    public AudioClip enemyHitClip;  // 敌人受伤 (攻击生效)
    //public AudioClip attackSwingClip; // (可选) 挥剑/发射的声音

    void Awake()
    {
        // 单例模式初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切换场景时保留音乐
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic(backgroundMusic);
    }

    // --- 播放背景音乐 ---
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;

        musicSource.clip = clip;
        musicSource.loop = true; // 开启循环
        musicSource.Play();
    }

    // --- 播放一次性音效 (通用) ---
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip); // PlayOneShot 允许声音重叠
    }

    // --- 专门用于状态切换的音效 ---
    public void PlayStateSwitchSound(int stateIndex)
    {
        // 确保索引在数组范围内
        if (stateSwitchClips != null && stateIndex < stateSwitchClips.Length)
        {
            PlaySFX(stateSwitchClips[stateIndex]);
        }
    }
}