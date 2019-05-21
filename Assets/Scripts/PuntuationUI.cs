using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PuntuationUI : MonoBehaviour
{
    public static PuntuationUI instance;

    private Text _text;

    private int mod(int k, int n) { return ((k %= n) < 0) ? k + n : k; }

    private void Awake()
    {
        instance = this;
        _text = GetComponent<Text>();
    }

    public void setPuntuation(int points)
    {
        _text.text = "GUAU: " + points;
    }

    public void setTextColor(Color c)
    {
        _text.color = c;
    }
}
