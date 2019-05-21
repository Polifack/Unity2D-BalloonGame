using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenuManager : MonoBehaviour
{
    public CustomButton b_restartGame;

    private void Awake()
    {
        if (b_restartGame == null)
        {
            Debug.LogError(" [!] Custom Button " + b_restartGame.name + " can't be null");
        }
    }

    private void Start()
    {
        b_restartGame.e_OnPointerDown += GameManager.singleton.GoToRestartGame;
    }
}