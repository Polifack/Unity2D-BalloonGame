using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public CustomButton b_startGame;
    public CustomButton b_gallery;
    public CustomButton b_shop;

    private void Awake()
    {
        if (b_startGame == null)
        {
            Debug.LogError(" [!] Custom Button " + b_startGame.name + " can't be null");
        }
        if (b_gallery == null)
        {
            Debug.LogError(" [!] Custom Button " + b_gallery.name + " can't be null");
        }
        if (b_shop == null)
        {
            Debug.LogError(" [!] Custom Button " + b_shop.name + " can't be null");
        }
    }

    private void Start()
    {
        b_startGame.e_OnPointerDown += GameManager.singleton.GoToStartGame;
        b_gallery.e_OnPointerDown += GameManager.singleton.GoToStartGame;
        b_shop.e_OnPointerDown += GameManager.singleton.GoToStartGame;
    }
}
