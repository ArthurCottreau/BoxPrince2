using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text textHeight;
    public TMP_Text textScore;
    public TMP_Text textRecord;
    public GameObject player;
    public byte scoreDecrease;
    public int scoreGain;

    private float score;
    private float height;
    private float maxHeight = 1;

    // pour obtenir le score ailleurs, pour Game Over par exemple
    public float GetScore()
    {
        return score;
    }
    // pour obtenir la hauteur max atteinte ailleurs, pour Game Over par exemple
    public float GetHeight()
    {
        return maxHeight;
    }
    void Start()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        textRecord.text = "Record : " + gameManager.highScore;
        score = 0;
    }

    void Update()
    {
        // calcul et affichage de la hauteur du joueur, à 2 décimales près
        height = player.transform.position.y;
        textHeight.text = "Hauteur : " + height.ToString("0.00") + "m";

        if (height > maxHeight) // check si hauteur max atteinte dépassée, pour gain de score
        {
            float diff = height - maxHeight;
            maxHeight = height;
            score += scoreGain * diff;
        }

        score -= scoreDecrease * Time.deltaTime; // chute du score avec le temps
        if (score < 0) score = 0; // pas de score négatif !

        textScore.text = "Score : " + Mathf.Round(score); // affichage du score, sans décimal
    }

    public void update_hscore(float newscore)
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (newscore > gameManager.highScore)
        {
            gameManager.highScore = newscore;
        }
    }
}
