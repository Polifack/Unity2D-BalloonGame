using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Custom implementation of Input Manager

//Goals: Asignar acciones a un conjunto de keys
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [System.Serializable]
    public class Input
    {
        public string nameInput;
        public string[] key;
        public float doublePressTimeout = 1f;
        private float _timeSinceClick;
        private float _lastClickTime;
        private bool _hasBeenDoublePressed = false;

        public bool isPressed()
        {
            for (int i = 0; i < key.Length; i++)
            {
                key[i].ToUpper();
                KeyCode kc = (KeyCode)System.Enum.Parse(typeof(KeyCode), key[i]);
                if (UnityEngine.Input.GetKey(kc) == true) return true;
            }
            return false;
        }

        public bool checkClick()
        {
            for (int i = 0; i < key.Length; i++)
            {
                key[i].ToUpper();
                KeyCode kc = (KeyCode)System.Enum.Parse(typeof(KeyCode), key[i]);
                if (UnityEngine.Input.GetKeyDown(kc) == true) return true;
            }
            return false;
        }

        float clicked = 0;
        float clicktime = 0;
        float clickdelay = 1f;

        public bool OnPointerDown()
        {
            if (checkClick())
            {
                clicked++;
                if (clicked == 1) clicktime = Time.time;
                if (clicked > 1 && Time.time - clicktime < clickdelay)
                {
                    clicked = 0;
                    clicktime = 0;
                    return true;
                }
                else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
            }
            return false;
        }
    }

    public Input[] inputs;

    private void Awake()
    {
        instance = this;

        foreach (Input input in inputs)
        {
            Debug.Log(input.nameInput);
            Debug.Log(input.key);
        }
    }

    public Input getInput(string inputName)
    {
        Input i = Array.Find(inputs, item => item.nameInput == inputName);
        return i;
    }

}
