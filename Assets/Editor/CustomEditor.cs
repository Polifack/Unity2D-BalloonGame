using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomEditor : MonoBehaviour
{
    [MenuItem("GameObject/BallonGame/PatrolEnemy", priority=10)]
    private static void CreatePrefab()
    {
        GameObject pPrefab = Instantiate(Resources.Load<GameObject>("PatrollingEnemy"), Vector3.zero, Quaternion.identity);
        pPrefab.name = "New Patrol Enemy";
    }
}
