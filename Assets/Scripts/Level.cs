using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "System/Level", order = 3)]
public class Level : ScriptableObject
{
    public GameObject levelScene;
    public int levelDificulty;
    public string levelName;
    public int height;
}
