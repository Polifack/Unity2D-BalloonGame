using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public static readonly int LEFT = 0;
    public static readonly int RIGHT = 1;
    public static readonly int CENTER = 2;

    public bool canMove = true;
    public float velocity = 1.5f;
    public float baloonWeight;
    public Balloon baloonPrefab;
    public Transform[] baloonsPositions;
    public Color[] colors;

    BalloonButtonManager[] _balloonButtons;
    Balloon[] _balloons;
    Rigidbody2D _rb;
    int _nBalloons;
    Vector2 _viewp;
    PuntuationManager _pointMgr;

    bool _isFalling = false;
    bool _isWraping = false;

    float _addForceTimeout = 2f;
    float _addForceC = 0f;
    float _nextForceX;
    float _nextForceY;

    Vector3 _startingPosition;


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
        _startingPosition = transform.position;
    }
    public void SetupBalloons()
    {
        _nBalloons = 3;
        _balloons = new Balloon[_nBalloons];

        for (int i = 0; i < _nBalloons; i++)
        {
            _balloons[i] = Instantiate(baloonPrefab, baloonsPositions[i]);
            _balloons[i].OnExplode += Balloon_OnExplode;
            _balloons[i].balloonId = i;
            _balloons[i].setColor(colors[i]);
        }
    }

    public void SetupBalloonButtons()
    {
        _balloonButtons = new BalloonButtonManager[_nBalloons];
        _balloonButtons = UIManager.singleton.GetActiveCanvas().GetComponentsInChildren<BalloonButtonManager>();
        for (int i = 0; i < _nBalloons; i++)
        {
            _balloonButtons[i].setupButton(_balloons[i]);
        }
    }

    public void SetupPlayer()
    {
        _isFalling = false;
        transform.position = _startingPosition;
    }

    private void Balloon_OnExplode(object sender, EventArgs e)
    {
        //Reducimos el numero de globos
        _nBalloons--;
        //Ejecutamos la funcion de re-arrangear
        Balloon b = (Balloon)sender;
        Destroy(b.gameObject);
        reArrangePosition(b.balloonId);
    }

    private float comptueX()
    {
        float left = 0f;
        float right = 0f;

        if ((_balloons[0] != null) && (_balloons[1] != null))
        {
            left = _balloons[0].airQuantity;
            right = _balloons[1].airQuantity;
        }
        return right - left;
    }

    private float computeY()
    {
        float speed = 1f;
        if (_balloons[2]!=null)
            speed = _balloons[2].airQuantity;
        return speed;
    }

    //Al rearrangear positions tambien hay que rearrangear los botones
    private void reArrangePosition(int position)
    {
        if (_nBalloons == 2)
        {
            if (position == 0)
            {
                swap(0, 2); //Si perdimos el globo izquierdo, cambiamos el superior por el izquierdo.
                _balloons[0].moveToNextBalloon(0);
                _balloons[0].balloonId = 0; //Actualizamos el 'id' del objeto
            }
            if (position == 1)
            {
                swap(1, 2); //Si perdimos el globo derecho, cambiamos el superior por el derecho.
                _balloons[1].moveToNextBalloon(1);  //Actualizamos la posicion del objeto
                _balloons[1].balloonId = 1; //Actualizamos el 'id' del objeto
            }
        }
        if (_nBalloons == 1)
        {
            if (position == 0)
            {
                swap(2, 1); //Si perdimos el globo izquierdo, cambiamos el derecho por el superior.
                _balloons[2].moveToNextBalloon(2); //Actualizamos la posicion del objeto
                _balloons[2].balloonId = 2; //Actualizamos el 'id' del objeto
            }
            if (position == 1)
            {
                swap(2, 0); //Si perdimos el globo derecho, cambiamos el izquierdo por el superior.
                _balloons[2].moveToNextBalloon(2);  //Actualizamos la posicion del objeto
                _balloons[2].balloonId = 2; //Actualizamos el 'id' del objeto
            }
        }
        if (_nBalloons == 0)
        {
            GameManager.singleton.GoToDeathScene();
        }
    }

    private void swap(int pos1, int pos2)
    {
        Balloon temp = _balloons[pos1];
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

    public void enableGravity()
    {
        _isFalling = true;
    }


    public void addForce(Vector2 force)
    {
        _addForceC = _addForceTimeout;
        _nextForceX = force.x;
        _nextForceY = force.y;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            _pointMgr.setPoints((int)Mathf.Floor(transform.position.y));
            ScreenWrap();
            transform.eulerAngles = new Vector3(0, 0, comptueX()* baloonWeight);

            if (_addForceC>0)
            {
                _rb.MovePosition(_rb.position + new Vector2(comptueX()+ _nextForceX, computeY()+ _nextForceY) * Time.deltaTime * velocity);
                _addForceC -= Time.deltaTime;

                _nextForceX = Mathf.MoveTowards(_nextForceX, 0, Time.deltaTime);
                _nextForceY = Mathf.MoveTowards(_nextForceY, 0, Time.deltaTime);
            }
            else
            {
                _rb.MovePosition(_rb.position + new Vector2(comptueX(), computeY()) * Time.deltaTime * velocity);
            }
            
        }
        if (_isFalling)
        {
            _rb.MovePosition(_rb.position + new Vector2(0, -5) * Time.deltaTime * velocity);
        }
    }
}
