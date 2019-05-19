using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager singleton;
    public UIScreen[] screens;

    private UIScreen _activeScreen;
    private Canvas _activeCanvas;

    private void Awake()
    {
        if (singleton != null)
        {
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void ChangeActiveScreen(string screenName)
    {
        UIScreen screen = Array.Find(screens, item => item.nameCanvas == screenName);

        if (screen == null)
        {
            Debug.LogWarning(" * Error: " + screenName + " screen not found");
            return;
        }
        if (_activeCanvas != null)
        {
            Destroy(_activeCanvas.gameObject);
        }
            
        Canvas currentCanvas = Instantiate(screen.source);
        _activeScreen = screen;
        _activeCanvas = currentCanvas;



    }
    public bool IsActiveScreen(string screenName)
    {
        return screenName == _activeScreen.nameCanvas;
    }
}
