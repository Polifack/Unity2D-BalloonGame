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
        InitializeGame(!debug);
    }

    public void GoToStartGame(object sender, EventArgs e)
    {
        StartCoroutine(StartGameCorroutine(1));
    }

    public void GoToRestartGame(object sender, EventArgs e)
    {
        InitializeGame(false);
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
        AudioManager.singleton.Stop("ostMexico");
        StartCoroutine(Pause(delay));
        Player.instance.enableGravity();
        CoinMananager.singleton.CreateCoins(Player.instance.getPoints(), Player.instance.transform.position);
        yield return new WaitForSeconds(0.2f);
        AudioManager.singleton.Play("sfxFall");
        Player.instance.reduceToCeroPoitns();
        yield return new WaitForSeconds(2);
        AudioManager.singleton.Play("sfxHit");
        CameraManager.singleton.Shake(0.5f);
        yield return new WaitForSeconds(0.5f);
        AudioManager.singleton.Play("ostScrapping");
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

    public void InitializeMainMenu()
    {
        Player.instance.SetupBalloons();

        UIManager.singleton.ChangeActiveScreen("uiMainMenu");
        CameraManager.singleton.setBlack();
    }

    public void InitializeGame(bool showMenu)
    {
        StartCoroutine(CameraManager.singleton.fadeIn(0.01f));
        Player.instance.SetupPlayer();
        CameraManager.singleton.FocusOnPlayer();
        LevelGenerator.instance.DestroyAll();
        AudioManager.singleton.StopAll();
        LevelGenerator.instance.Generate();
        ParallaxManager.singleton.startAllParallaxes();
        ParallaxManager.singleton.resetAllParallaxes();
        AudioManager.singleton.Play("ostMexico");
        if (showMenu)
        {
           InitializeMainMenu();
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
