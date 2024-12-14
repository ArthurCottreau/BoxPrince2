using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelChunks", menuName = "LevelChunks")]
public class LevelChunks : ScriptableObject
{
    public GameObject[] Chunks;
}
