using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    public float DamageCooldown = 2f;
    float _damageC = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Balloon>() != null) {
            //Is a balloon!
            if (_damageC < DamageCooldown)
            {
                collision.gameObject.GetComponent<Balloon>().Explode(this, null);
                _damageC = DamageCooldown;
            }
        }
    }

    private void Update()
    {
        if (_damageC >= DamageCooldown) _damageC += Time.deltaTime;
    }
}
