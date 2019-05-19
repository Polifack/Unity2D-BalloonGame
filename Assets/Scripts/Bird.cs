using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float velocity = 2f;
    int _direction = 1;
    Rigidbody2D _rb;
    BoxCollider2D _bc;

    public EventHandler onCollison;

    private void OnBecameInvisible()
    {
        _direction *= -1;   
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Baloon b = collision.gameObject.GetComponent<Baloon>();
        if (b != null)
        {
            Debug.Log("collided with " + b.id);
            onCollison(this, null);
            _bc.enabled = false;
        }
    }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
    }
    private void FixedUpdate()
    {
        _rb.MovePosition(new Vector2(_rb.position.x + velocity * Time.deltaTime * _direction, _rb.position.y));
        if (transform.position.y + GameManager.SCREEN_WIDHT < Player.instance.transform.position.y)
        {
            Destroy(gameObject);
        }
    }

}
