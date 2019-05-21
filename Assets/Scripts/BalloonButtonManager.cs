using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BalloonButtonManager : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,
    IPointerUpHandler
{
    public event EventHandler OnClickDown;
    public event EventHandler OnClickUp;
    public event EventHandler OnDoubleClick;

    float _clicked = 0;
    float _clickTime = 0;
    float clickDelay = 1f;

    Animator _anim;
    Image _img;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _img = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _clicked++;
        if (_clicked == 1) _clickTime = Time.time;
        if (_clicked > 1 && Time.time - _clickTime < clickDelay)
        {
            ExplodeButton();
            OnDoubleClick(this, null);
        }
        else if (_clicked > 2 || Time.time - _clickTime > 1) _clicked = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClickDown(this, null);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnClickUp(this, null);
    }
    
    public void setupButton(Balloon b)
    {
        OnClickDown += b.StartDeinflate;
        OnClickUp += b.StopDeinflate;
        OnDoubleClick += b.Explode;
        _img.color = b.getColor();
    }

    public void ExplodeButton()
    {
        _clicked = 0;
        _clickTime = 0;
        _anim.Play("Explode");
    }
}
