using TMPro;
using UnityEngine;

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
    private float diff_mult;

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

        // Configure le multiplicateur de score
        switch (gameManager.difficulty)
        {
            case 0:
                diff_mult = 0.75f;
                break;
            case 1:
                diff_mult = 1.0f;
                break;
            case 2:
                diff_mult = 1.25f;
                break;
            default:
                diff_mult = 0;
                Debug.Log("Erreur, la valeur li� � la difficult� est mal configurer !");
                break;
        }
    }

    void Update()
    {
        // calcul et affichage de la hauteur du joueur, � 2 d�cimales pr�s
        height = player.transform.position.y;
        textHeight.text = "Hauteur : " + height.ToString("0.00") + "m";

        if (height > maxHeight) // check si hauteur max atteinte d�pass�e, pour gain de score
        {
            float diff = height - maxHeight;
            maxHeight = height;
            score += (scoreGain * diff) * diff_mult;
        }

        score -= scoreDecrease * Time.deltaTime; // chute du score avec le temps
        if (score < 0) score = 0; // pas de score n�gatif !

        textScore.text = "Score : " + Mathf.Round(score); // affichage du score, sans d�cimal
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
