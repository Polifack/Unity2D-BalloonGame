using UnityEngine;

public class TouchInputSystem : MonoBehaviour
{
    //Referencias a los botones
    private CustomButton b_left;
    private CustomButton b_right;

    private void Awake()
    {
        //Cargar los botones
        CustomButton[] cb = GetComponentsInChildren<CustomButton>();
        b_right = cb[0];
        b_left = cb[1];
    }

    private void Start()
    {

        /* Estas funciones deberían interactuar con el player con acciones como "MoveLeft" o "MoveRigh" y
         * dejar que el player decida como managear dichas interacciones debido a que no es asunto del 
         * controlador de movimiento la forma en la que se managea.
         */

        b_right.e_OnPointerDown += Player.instance.OnStartMovingRight;
        b_right.e_OnPointerUp += Player.instance.OnStopMovingRight;
        b_right.e_OnDoubleClick += Player.instance.OnExplodeRight;

        b_left.e_OnPointerDown += Player.instance.OnStartMovingLeft;
        b_left.e_OnPointerUp += Player.instance.OnStopMovingLeft;
        b_left.e_OnDoubleClick += Player.instance.OnExplodeLeft;
    }
}
