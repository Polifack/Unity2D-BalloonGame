using TMPro;
using UnityEngine;

public class DeathMenuManager : MonoBehaviour
{
    public CustomButton b_restartGame;
    public TextMeshProUGUI t_Puntuation;

    private void Awake()
    {
        if (b_restartGame == null)
        {
            Debug.LogError(" [!] Custom Button " + b_restartGame.name + " can't be null");
        }
        if (t_Puntuation == null)
        {
            Debug.LogError(" [!] TextMesh " + t_Puntuation.name + " can't be null");
        }
    }

    private void Start()
    {
        b_restartGame.e_OnPointerDown += GameManager.singleton.GoToRestartGame;
        t_Puntuation.SetText( "SCORE: "+Player.instance.getPoints());
    }
}