using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Level[] levels;
    public static LevelGenerator instance;

    GameObject _levelContainer;

    private void Awake()
    {
        if (instance == null) instance = this;

    }

    public void DestroyAll()
    {
        Destroy(_levelContainer);
    }

    public void Generate()
    {
        int position = 0;
        _levelContainer = new GameObject();

        foreach (Level l in levels)
        {
            position += l.height;
            GameObject go = Instantiate(l.levelScene, new Vector3(0, position, 0), Quaternion.identity);
            go.transform.SetParent(_levelContainer.transform);
        }
    }
}
