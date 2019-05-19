using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Player target;

    private void FixedUpdate()
    {
        transform.position += new Vector3(0, target.velocity * Time.deltaTime, 0);
    }
}
