using System;
using System.Collections;
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
        InitializeGame();
    }

    public void GoToStartGame(object sender, EventArgs e)
    {
        StartCoroutine(StartGameCorroutine(1));
    }

    public void GoToRestartGame(object sender, EventArgs e)
    {
        InitializeGame();
    }

    public void GoToDeathScene()
    {
        StartCoroutine(StartDeathCorroutine(1));
    }

    private IEnumerator StartDeathCorroutine(int delay)
    {
        Player.instance.canMove = false;
        CameraManager.singleton.canMove = false;
        ParallaxManager.singleton.stopAllParallaxes();
        StartCoroutine(Pause(delay));
        Player.instance.enableGravity();
        yield return new WaitForSeconds(delay);
        UIManager.singleton.ChangeActiveScreen("uiDeath");
        yield return null;
    }

    private IEnumerator StartGameCorroutine(int delay)
    {
        UIManager.singleton.ChangeActiveScreen("uiDefault");
        Player.instance.SetupBalloonButtons();
        yield return new WaitForSeconds(delay);
        Player.instance.canMove = true;
        CameraManager.singleton.canMove = true;
        yield return null;
    }

    private IEnumerator Pause(int p)
    {
        Time.timeScale = 0.0001f;
        float pauseEndTime = Time.realtimeSinceStartup + 1;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1;
    }

    public void InitializeGame()
    {
        Player.instance.SetupPlayer();
        CameraManager.singleton.FocusOnPlayer();
        LevelGenerator.instance.DestroyAll();
        LevelGenerator.instance.Generate();
        ParallaxManager.singleton.startAllParallaxes();
        if (!debug)
        {
            Player.instance.SetupBalloons();
            StartCoroutine(CameraManager.singleton.fadeIn(0.01f));
            UIManager.singleton.ChangeActiveScreen("uiMainMenu");
            CameraManager.singleton.setBlack();
            AudioManager.singleton.Play("ostMexico");
        }
        else
        {
            Player.instance.SetupBalloons();
            UIManager.singleton.ChangeActiveScreen("uiDefault");
            Player.instance.SetupBalloonButtons();
            Player.instance.canMove = true;
            CameraManager.singleton.canMove = true;
        }
    }
}
