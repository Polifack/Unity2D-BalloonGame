using System;
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

    //Setup Functions
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

    public void InitializePlayer()
    {
        _isFalling = false;
        transform.position = _startingPosition;
    }

    public Balloon getRightBalloon()
    {
        return _balloons[0];
    }
    public Balloon getLeftBalloon()
    {
        return _balloons[1];
    }


    private void Balloon_OnExplode(object sender, EventArgs e)
    {   
        //Reducimos el numero de globos
        _nBalloons--;
        //Ejecutamos la funcion de re-arrangear
        Balloon b = (Balloon)sender;
        Destroy(b.gameObject);
        if (_nBalloons == 0)
        {
            GameManager.singleton.GoToDeathScene();
        }
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

    public int getPoints()
    {
        return _pointMgr.getPoints();
    }

    public void reduceToCeroPoitns()
    {
        StartCoroutine(_pointMgr.reduceToCero());
    }

    public void addForce(Vector2 force)
    {
        _addForceC = _addForceTimeout;
        _nextForceX = force.x;
        _nextForceY = force.y;
    }

    private float xDirection;

    public void OnStartMovingLeft(object sender, EventArgs args)
    {
        xDirection = -1;
    }
    public void OnStartMovingRight(object sender, EventArgs args)
    {
        xDirection = 1;
    }

    public void OnStopMovingLeft(object sender, EventArgs args)
    {
        xDirection = 0;
    }
    public void OnStopMovingRight(object sender, EventArgs args)
    {
        xDirection = 0;
    }

    public void OnExplodeLeft(object sender, EventArgs args)
    {
        _balloons[0].Explode(null, EventArgs.Empty);
    }
    public void OnExplodeRight(object sender, EventArgs args)
    {
        _balloons[1].Explode(null, EventArgs.Empty);
    }

    

    private void FixedUpdate()
    {   
        //Normal game state
        if (canMove)
        {
            //Points
            _pointMgr.setPoints(transform.position.y);

            //Screen Wrap
            ScreenWrap();

            //Rotation
            transform.eulerAngles = new Vector3(0, 0, comptueX()* baloonWeight);

            //Balloon Force
            if (_addForceC>0)
            {
                _rb.MovePosition(_rb.position + new Vector2(comptueX()+ _nextForceX, computeY()+ _nextForceY) * Time.deltaTime * velocity);
                _addForceC -= Time.deltaTime;

                _nextForceX = Mathf.MoveTowards(_nextForceX, 0, Time.deltaTime);
                _nextForceY = Mathf.MoveTowards(_nextForceY, 0, Time.deltaTime);
            }

            //Normal Movement
            else
            {
                _rb.MovePosition(_rb.position + new Vector2(xDirection, 1) * Time.deltaTime * velocity);
            }

        }

        //Death State
        if (_isFalling)
        {
            _rb.MovePosition(_rb.position + new Vector2(0, -5) * Time.deltaTime * velocity);
        }
    }
}
