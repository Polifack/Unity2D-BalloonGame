using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Tooltip("Tamaño de la imagen usada dividido entre 10")]
    public float backgroundSize;
    public float parallaxSpeed;
    public Transform cameraTransform;
    public float viewPort = 10; 
    

    private int downBck;
    private int upBckg;
    private Transform[] backgrounds; //Array of backgrounds to be parallaxed
    private float _lastCameraY;
    private bool _canParallax;
    private Vector3 _initialPos;
    private Vector3[] _initialBackgrounds;


    private int mod(int k, int n) { return ((k %= n) < 0) ? k + n : k; }


    // Start is called before the first frame update
    void Awake ()
    { 
        _lastCameraY = cameraTransform.position.y;
        backgrounds = new Transform[transform.childCount];
        _initialBackgrounds = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            backgrounds[i] = transform.GetChild(i);
            _initialBackgrounds[i] = transform.GetChild(i).position;
        }

        downBck = 0;
        _initialPos = transform.position;
        upBckg = transform.childCount - 1;

    }

    private void ScrollUp()
    {
        //Movemos el bck de abajo a la "vieja" posicion del bckg superior
        backgrounds[downBck].position = new Vector3(
            backgrounds[downBck].position.x, 
            (backgrounds[upBckg].position.y + backgroundSize),
            backgrounds[downBck].position.z
        );

        //Actualizamos posiciones
        downBck = upBckg; //abajo -> arriba
        upBckg--; //arriba -> medio
        upBckg = mod(upBckg, backgrounds.Length); 
    }

    public void resetParallax()
    {
        transform.position = _initialPos;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).position = _initialBackgrounds[i];
        }
    }

    public void SetParallax(bool b)
    {
        _canParallax = b;
    }
    
    private void FixedUpdate()
    {
        if (_canParallax)
        {
            float deltaY = cameraTransform.position.y - _lastCameraY;
            transform.position += Vector3.down * (parallaxSpeed);
            _lastCameraY = cameraTransform.position.y;

            if (cameraTransform.position.y > (backgrounds[downBck].position.y) + viewPort)
                ScrollUp();
        }
    }
}
