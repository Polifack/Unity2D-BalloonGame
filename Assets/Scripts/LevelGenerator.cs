using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Level[] levels;
    public static LevelGenerator instance;

    private void Awake()
    {
        if (instance == null) instance = this;

    }

    public void Generate()
    {
        int position = 0;

        foreach (Level l in levels)
        {
            position += l.height;
            Instantiate(l.levelScene, new Vector3(0, position, 0), Quaternion.identity);
        }
    }
}
