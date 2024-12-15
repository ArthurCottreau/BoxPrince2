using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GraphicsBuffer;

public class GameOver : MonoBehaviour
{
    [SerializeField] private UIManager manager;
    [SerializeField] private GameObject goui; // Game Over User Interface
    [SerializeField] private TMP_Text display_score;
    [SerializeField] private TMP_Text display_height;
    private ScoreManager scoreManager;

    private int finalScore;
    private float finalHeight;

    // Audio
    private AudioSource sfx;
    [SerializeField] private AudioClip audioLose;

    private void Start()
    {
        scoreManager = GameObject.Find("GameManager").GetComponent<ScoreManager>();

        sfx = gameObject.GetComponent<AudioSource>();
        GameManager gameMan = GameObject.Find("GameManager").GetComponent<GameManager>();
        sfx.volume = gameMan.sfxVolume / 100 / gameMan.sfxOffset;
    }

    public void Death() // public pour être appelée par des objets externes
    {
        // Récupère le score puis l'affiche sur l'UI GameOver
        finalScore = Mathf.RoundToInt(manager.GetScore());
        finalHeight = manager.GetHeight();
        display_score.text = "Score : " + finalScore;
        display_height.text = "Hauteur : " + finalHeight.ToString("0.00") + "m";
        goui.SetActive(true);

        sfx.clip = audioLose;
        sfx.Play();
        AudioSource bgm = GameObject.Find("GameManager").GetComponent<AudioSource>();
        bgm.volume /= 3;

        // Sauvegarde le Score
        scoreManager.newScore(finalScore, finalHeight);

        // Met à jour le highscore dans le GameManager
        manager.update_hscore(finalScore);
    }
    public int GetFinalScore()
    {
        return finalScore;
    }
    public float GetFinalHeight()
    {
        if (finalHeight == 0)
        {
            Debug.LogWarning("Attention, la hauteur récupérée est de 0 : est-ce du à un appel de la fonction avant un Game Over ?");
        }
        return finalHeight;
    }

    public void nextScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
