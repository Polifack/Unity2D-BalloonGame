using System.Collections;
using UnityEngine;

public class PuntuationManager
{
    int points;

    public PuntuationManager()
    {
        points = 0;
    }
    public void setPoints(float f)
    {
        points = Mathf.FloorToInt(f);
        PuntuationUI.instance.setPuntuation(points);
    }
    public int getPoints()
    {
        return points;
    }
    public IEnumerator reduceToCero()
    {
        PuntuationUI.instance.setTextColor(Color.red);
        while (points > 0)
        {
            points--;
            PuntuationUI.instance.setPuntuation(points);
            yield return null;
        }
    }
}
