using System.Collections.Generic;

[System.Serializable]
public class ScoreData
{
    public List<ElementScore> scoreList = new List<ElementScore>();
}

[System.Serializable]
public class ElementScore
{
    public int scoreJoueur;
    public float hauteurJoueur;

    public ElementScore(int score, float hauteur)
    {
        scoreJoueur = score;
        hauteurJoueur = hauteur;
    }
}
