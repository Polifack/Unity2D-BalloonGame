using System.Collections;
using System.Collections.Generic;
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

        if (mod(points, 10) == 0) StartCoroutine(spicyEffect());
    }

    public IEnumerator spicyEffect()
    {
        _text.color = Color.yellow;
        yield return new WaitForSeconds(0.5f);
        _text.color = Color.white;
    }




}
