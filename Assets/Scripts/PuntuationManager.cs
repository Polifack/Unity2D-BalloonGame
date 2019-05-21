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
        int temp_points = points;
        while (points > 0)
        {
            temp_points--;
            PuntuationUI.instance.setPuntuation(temp_points);
            yield return null;
        }
    }
}
