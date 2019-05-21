using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    public Parallax[] parallaxes;
    public static ParallaxManager singleton;

    private void Awake()
    {
        singleton = this;
    }

    public void stopAllParallaxes()
    {
        foreach (Parallax p in parallaxes)
        {
            p.SetParallax(false);
        }
    }

    public void startAllParallaxes()
    {
        foreach (Parallax p in parallaxes)
        {
            p.SetParallax(true);
        }
    }
}
