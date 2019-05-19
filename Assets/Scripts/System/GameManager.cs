using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public bool debug = true;

    public static int SCREEN_HEIGHT;
    public static int SCREEN_WIDHT;

    private void Awake()
    {
        SCREEN_HEIGHT = Camera.main.pixelHeight;
        SCREEN_WIDHT = Camera.main.pixelWidth;
        Debug.Log(" * Starting game in a " + SCREEN_WIDHT + "x" + SCREEN_HEIGHT + " screen.");
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

    private void Start()
    {
        Player.instance.canMove = true;
        if (!debug)
        {
            StartCoroutine(StartGameCorroutine(5));
            ScreenEffects.singleton.setBlack();
            AudioManager.singleton.Play("ostRolling");
        }
        UIManager.singleton.ChangeActiveScreen("uiDefault");
    }

    private IEnumerator StartGameCorroutine(int delay)
    {
        StartCoroutine(ScreenEffects.singleton.fadeIn(0.01f));
        yield return new WaitForSeconds(delay);
        Player.instance.canMove = true;
        yield return null;
    }
}
