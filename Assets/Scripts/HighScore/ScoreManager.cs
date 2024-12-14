using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private ScoreData scoreData = new ScoreData();
    private string dataPath = Application.dataPath + "/savescore.json";

    private void Awake()
    {
        // Si un fichier de sauvegarde n'existe pas, le jeu en cr�ais un
        // Sinon il charge la sauvegarde existante
        if (!File.Exists(dataPath))
        {
            string jsondata = JsonUtility.ToJson(scoreData);
            File.WriteAllText(dataPath, jsondata);
        }
        else
        {
            scoreData = LoadScores();
        }
    }

    public ScoreData getScores()
    {
        return scoreData; // R�cup�re la liste de tout les scores
    }

    public void newScore(int newScore, float newHauteur)
    {
        // Ajoute un score � la liste existante
        scoreData.scoreList.Add(new ElementScore(newScore, newHauteur));

        // Puis convertis se score pour �tre sauvegarder dans le ficher .json
        string jsondata = JsonUtility.ToJson(scoreData);
        File.WriteAllText(dataPath, jsondata);
    }

    private ScoreData LoadScores()
    {
        // Charge la sauvegarde
        string jsondata = File.ReadAllText(dataPath);
        return JsonUtility.FromJson<ScoreData>(jsondata);
    }
}
