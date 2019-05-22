using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Sprite normalState;
    public Sprite pressState;
    public Sprite hoverState;

    public event EventHandler e_OnPointerDown;
    public event EventHandler e_OnPointerUp;
    public event EventHandler e_OnPointerEnter;
    public event EventHandler e_OnPointerExit;
    public event EventHandler e_OnDoubleClick;

    Image _img;

    int _clicked = 0;       //Counter to check how many times we pressed the button
    float _clickTime = 0;   //Counter to check time between clicks
    float _clickDelay = 2f; //Time allowed between clicks

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
    public void OnPointerClick(PointerEventData eventData)
    {
        _clicked++;
        if (_clicked == 1) _clickTime = Time.time;
        if (_clicked > 1 && Time.time - _clickTime < _clickDelay)
        {
            _clicked = 0;
            _clickTime = 0;
            e_OnDoubleClick?.Invoke(this, null);
        }
        else if (_clicked > 2 || Time.time - _clickTime > 1) _clicked = 0;
    }

}
