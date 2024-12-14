using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int difficulty;
    public float sfxVolume;
    public float sfxOffset; // divise le volume sfx par cette valeur. Défaut = 1
    public float bgmVolume;
    public float bgmOffset; // divise le volume bgm par cette valeur. Défaut = 1
    public float highScore;

    // Audio
    public AudioSource bgm;
    public AudioClip bgmMenu;
    public AudioClip bgmGame;
    private int sceneID;

    public void BgmVolume()
    {
        bgm.volume = bgmVolume / 100 / bgmOffset;
    }

    private void Awake()
    {
        int numGameObject = FindObjectsOfType<GameManager>().Length;
        if (numGameObject != 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);  // Fait en sorte que le GameManager soit persistent au travers les scènes
            bgmGame.LoadAudioData();
            sceneID = 0;
            bgm.clip = bgmMenu;
            bgm.Play();
        }
    }

    private void FixedUpdate()
    {
        if (sceneID != SceneManager.GetActiveScene().buildIndex)
        {
            bgm.Stop();
            sceneID = SceneManager.GetActiveScene().buildIndex;
            if (bgm.clip == bgmMenu) bgm.clip = bgmGame;
            else bgm.clip = bgmMenu;
            bgm.Play();
        }
    }
}