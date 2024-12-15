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
    public string diffJoueur;

    public ElementScore(int score, float hauteur, string diff)
    {
        scoreJoueur = score;
        hauteurJoueur = hauteur;
        diffJoueur = diff;
    }
}
