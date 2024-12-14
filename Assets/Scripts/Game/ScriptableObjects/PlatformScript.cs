using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Platform", menuName = "Platform/Create New Platform")]
public class PlatformScript : ScriptableObject
{
    public int id;
    public Sprite icon;
    public GameObject prefab;

    public float decayTime = 10f; //Temps d'errosion du block
    public float decayCollision = 1f; //Temps soustrait à l'errosion du block quand entre en contact avec joueur
    public float decayMultiplier = 1f; //Multiplicateur à la vitesse d'errosion quand block est en contact avec joueur
}
