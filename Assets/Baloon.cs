﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baloon : MonoBehaviour
{
    public float airP = 1f;
    public float airInflat = 0.1f;
    public int id;
    public float deinflateCd = 1f;
    public Color color;
    private bool _hasToMove = false;
    private int _nextPositionIndex;
    private float _moveSpeed = 1f;
    public float _deinflateCounter = 0f;
    private SpriteRenderer sr;

    private void Awake()
    {
        _deinflateCounter = deinflateCd;
        sr = GetComponent<SpriteRenderer>();
    }

    public event EventHandler OnExplode;

    public void setColor()
    {
        sr.color = color;
    }

    public void Inflate()
    {
        airP = Mathf.Clamp((airP + airInflat * Time.deltaTime), 0f, 1f);
        transform.position += new Vector3(0, 0.1f * Time.deltaTime, 0);
    }

    public void DeInflate()
    {
        Debug.Log("DeInflating! "+ id);
        airP = Mathf.Clamp((airP - airInflat * Time.deltaTime), 0f, 2f);
        transform.position -= new Vector3(0, 0.1f * Time.deltaTime, 0);
        _deinflateCounter = 0f;
    }

    public void Explode()
    {
        airP = 0;
        OnExplode(this, EventArgs.Empty);
        //Destroy(gameObject);
    }

    public void moveToNextBalloon(int next)
    {
        _hasToMove = true;
        _nextPositionIndex = next;
    }

    private void FixedUpdate()
    {
        //Cambiar por Animator
        transform.localScale = new Vector3(airP, airP, transform.localScale.z);

        if (airP < 1f && _deinflateCounter > deinflateCd)
        {
            Inflate();
        }
        else
        {
            _deinflateCounter += Time.deltaTime;
        }

        //Slowly movement
        if (_hasToMove)
        {
            transform.position = Vector3.Lerp(transform.position, Player.instance.getBallonPosition(_nextPositionIndex), _moveSpeed*Time.deltaTime);
        }
    }
}
