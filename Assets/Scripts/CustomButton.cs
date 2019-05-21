using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite normalState;
    public Sprite pressState;
    public Sprite hoverState;

    public event EventHandler e_OnPointerDown;
    public event EventHandler e_OnPointerUp;
    public event EventHandler e_OnPointerEnter;
    public event EventHandler e_OnPointerExit;

    private Image _img;

    private void Awake()
    {
        _img = GetComponent<Image>();
        if (_img == null)
        {
            Debug.LogError(" [!] Image in Custom Button " + gameObject.name + " can't be null");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Change Sprite
        _img.sprite = pressState;
        //Invoke event if not null
        e_OnPointerDown?.Invoke(this, null);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _img.sprite = hoverState;
        e_OnPointerEnter?.Invoke(this, null);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _img.sprite = normalState;
        e_OnPointerExit?.Invoke(this, null);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _img.sprite = normalState;
        e_OnPointerUp?.Invoke(this, null);
    }
}
