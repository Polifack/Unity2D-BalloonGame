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
        LevelGenerator.instance.Generate();
        if (!debug)
        {
            Player.instance.SetupBalloons();
            StartCoroutine(ScreenEffects.singleton.fadeIn(0.01f));
            UIManager.singleton.ChangeActiveScreen("uiMainMenu");
            ScreenEffects.singleton.setBlack();
            AudioManager.singleton.Play("ostMexico");
        }
        else
        {
            Player.instance.SetupBalloons();
            Player.instance.SetupBalloonButtons();
            UIManager.singleton.ChangeActiveScreen("uiDefault");
            Player.instance.canMove = true;
            ScreenEffects.singleton.canMove = true;
        }
    }

    public void GoToStartGame()
    {
        StartCoroutine(StartGameCorroutine(1));
    }

    private IEnumerator StartGameCorroutine(int delay)
    {
        UIManager.singleton.ChangeActiveScreen("uiDefault");
        Player.instance.SetupBalloonButtons();
        yield return new WaitForSeconds(delay);
        Player.instance.canMove = true;
        ScreenEffects.singleton.canMove = true;
        yield return null;
    }
}
