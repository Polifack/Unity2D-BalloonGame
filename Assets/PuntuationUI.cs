using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuntuationUI : MonoBehaviour
{
    public static PuntuationUI instance;

    private Text _text;
    private void Awake()
    {
        instance = this;
        _text = GetComponent<Text>();
    }

    public void setPuntuation(int points)
    {
        _text.text = "Puntos : " + points;
    }




}
