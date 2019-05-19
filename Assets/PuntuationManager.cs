using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntuationManager
{
    int points;

    public PuntuationManager()
    {
        points = 0;
    }
    public void setPoints(int n)
    {
        points = n;
        PuntuationUI.instance.setPuntuation(points);
    }

}
