using System.Linq;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject content;

    private void Start()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ScoreManager scoreManager = GameObject.Find("GameManager").GetComponent<ScoreManager>();

        ScoreData data = scoreManager.getScores();

        data.scoreList = data.scoreList.OrderByDescending(go => go.scoreJoueur).ToList<ElementScore>();
        
        // Vérifie si la liste est vide, si elle n'est pas créais les éléments du leaderboard
        if (data.scoreList.Count > 0)
        {
            gameManager.highScore = data.scoreList[0].scoreJoueur; // Met à jour le highscore dans le GameManager

            for (int i = 0; i < data.scoreList.Count; i++)
            {
                GameObject inst = Instantiate(prefab, content.transform.position, Quaternion.identity, content.transform);
                inst.transform.Find("TextScore").GetComponent<TextMeshProUGUI>().text = "Score : " + data.scoreList[i].scoreJoueur;
                inst.transform.Find("TextHeight").GetComponent<TextMeshProUGUI>().text = "Hauteur : " + data.scoreList[i].hauteurJoueur.ToString("0.00") + "m";
            }
        }

        // Modifie la taille de la zone scroll dépendant du nombre d'éléments dans la liste
        RectTransform newcont = content.GetComponent<RectTransform>();
        newcont.sizeDelta = new Vector2(0, data.scoreList.Count * 60 + 10);
    }
}