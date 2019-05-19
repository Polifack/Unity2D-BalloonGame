using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public bool canMove = true;
    public float velocity = 1.5f;
    public float baloonWeight;
    public Baloon baloonPrefab;
    public Transform[] baloonsPositions;
    public Color[] colores;

    BalloonButtonManager[] _balloonButtons;
    Baloon[] _balloons;
    Rigidbody2D _rb;
    int _nBalloons;
    Vector2 _viewp;
    bool _isWraping = false;
    PuntuationManager _pointMgr;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning(" * Only one player per scene!");
        }

        _rb = GetComponent<Rigidbody2D>();
        _pointMgr = new PuntuationManager();
        _viewp = Camera.main.WorldToViewportPoint(transform.position);
    }

    private void Start()
    {
        _nBalloons = 3;
        _balloons = new Baloon[_nBalloons];
        _balloonButtons = new BalloonButtonManager[_nBalloons];
        _balloonButtons = UIManager.singleton.GetActiveCanvas().GetComponentsInChildren<BalloonButtonManager>();

        for (int i = 0; i<_nBalloons; i++)
        {
            _balloons[i] = Instantiate(baloonPrefab, baloonsPositions[i]);
            _balloons[i].OnExplode += Balloon_OnExplode;
            _balloons[i].id = i;
            _balloons[i].setColor(colores[i]);

            _balloonButtons[i].OnClickDown += _balloons[i].StartDeinflate;
            _balloonButtons[i].OnClickUp += _balloons[i].StopDeinflate;
            _balloonButtons[i].OnDoubleClick +=  _balloons[i].Explode;
            _balloonButtons[i].SetColor(colores[i]);
        }
    }    

    private void Balloon_OnExplode(object sender, EventArgs e)
    {
        //Reducimos el numero de globos
        _nBalloons--;
        //Ejecutamos la funcion de re-arrangear
        Baloon b = (Baloon)sender;
        Destroy(b.gameObject);
        rearrange(b.id);
    }

    private float comptueX()
    {
        float left = 0f;
        float right = 0f;

        if ((_balloons[0] != null) && (_balloons[1] != null))
        {
            left = _balloons[0].airP;
            right = _balloons[1].airP;
        }
        return right - left;
    }

    private float computeY()
    {
        float speed = 1f;
        if (_balloons[2]!=null)
            speed = _balloons[2].airP;
        return speed;
    }

    private void rearrange(int position)
    {
        if (_nBalloons == 2)
        {
            if (position == 0)
            {
                swap(0, 2); //Si perdimos el globo izquierdo, cambiamos el superior por el izquierdo.
                _balloons[0].moveToNextBalloon(0);
                _balloons[0].id = 0; //Actualizamos el 'id' del objeto
            }
            if (position == 1)
            {
                swap(1, 2); //Si perdimos el globo derecho, cambiamos el superior por el derecho.
                _balloons[1].moveToNextBalloon(1);  //Actualizamos la posicion del objeto
                _balloons[1].id = 1; //Actualizamos el 'id' del objeto
            }

            //Si perdimos el globo superior no hacemos nada
        }
        if (_nBalloons == 1)
        {
            if (position == 0)
            {
                swap(2, 1); //Si perdimos el globo izquierdo, cambiamos el derecho por el superior.
                _balloons[2].moveToNextBalloon(2); //Actualizamos la posicion del objeto
                _balloons[2].id = 2; //Actualizamos el 'id' del objeto
            }
            if (position == 1)
            {
                swap(2, 0); //Si perdimos el globo derecho, cambiamos el izquierdo por el superior.
                _balloons[2].moveToNextBalloon(2);  //Actualizamos la posicion del objeto
                _balloons[2].id = 2; //Actualizamos el 'id' del objeto
            }
        }
    }

    private void swap(int pos1, int pos2)
    {
        Baloon temp = _balloons[pos1];
        _balloons[pos1] = _balloons[pos2];
        _balloons[pos2] = temp;
    }

    private void ScreenWrap()
    {
        _viewp = Camera.main.WorldToViewportPoint(transform.position);
        if (!_isWraping && (_viewp.x > 1 || _viewp.x < 0))
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
            _isWraping = true;
        }
        if (_isWraping && (_viewp.x > 0.15 && _viewp.x < 0.85))
        {
            _isWraping = false;
        }
    }

    public Vector3 getBallonPosition(int ballonPos)
    {
        return baloonsPositions[ballonPos].position;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            _pointMgr.setPoints((int)Mathf.Floor(transform.position.y));
            ScreenWrap();
            transform.eulerAngles = new Vector3(0, 0, comptueX()* baloonWeight);
            _rb.MovePosition(_rb.position + new Vector2(comptueX(), computeY()) * Time.deltaTime * velocity);
        }
    }
}
