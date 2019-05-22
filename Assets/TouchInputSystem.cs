using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputSystem : MonoBehaviour
{
    private CustomButton b_left;
    private CustomButton b_right;

    private void Awake()
    {
        CustomButton[] cb = GetComponentsInChildren<CustomButton>();
        b_right = cb[0];
        b_left = cb[1];
    }

    private void Start()
    {

        /* Estas funciones deberían interactuar con el player con acciones como "MoveLeft" o "MoveRigh" y
         * dejar que el player decida como managear dichas interacciones debido a que no es asunto del 
         * controlador de movimiento la forma en la que se managea
         */

        b_right.e_OnPointerDown += Player.instance.getRightBalloon().StartDeinflate;
        b_right.e_OnPointerUp += Player.instance.getRightBalloon().StopDeinflate;
        b_right.e_OnDoubleClick += Player.instance.getRightBalloon().Explode;

        b_left.e_OnPointerDown += Player.instance.getLeftBalloon().StartDeinflate;
        b_left.e_OnPointerUp += Player.instance.getLeftBalloon().StopDeinflate;
        b_left.e_OnDoubleClick += Player.instance.getLeftBalloon().Explode;
    }
}
