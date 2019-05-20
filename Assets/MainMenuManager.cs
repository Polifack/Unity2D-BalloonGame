using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button startGameButton;

    private void Awake()
    {
        startGameButton.onClick.AddListener(()=>GameManager.singleton.GoToStartGame());
    }
}
