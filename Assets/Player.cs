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

    Baloon[] _baloons;
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

        _nBalloons = 3;
        _baloons = new Baloon[_nBalloons];

        //Inicialización de los globos. 1-> Izda. 2-> Dcha. 3-> Centro.

        _baloons[0] = Instantiate(baloonPrefab, baloonsPositions[0]);
        _baloons[0].OnExplode += Balloon_OnExplode;
        _baloons[0].id = 0;
        _baloons[0].color = Color.red;
        _baloons[0].setColor();

        _baloons[1] = Instantiate(baloonPrefab, baloonsPositions[1]);
        _baloons[1].OnExplode += Balloon_OnExplode;
        _baloons[1].id = 1;
        _baloons[1].color = Color.blue;
        _baloons[1].setColor();

        _baloons[2] = Instantiate(baloonPrefab, baloonsPositions[2]);
        _baloons[2].OnExplode += Balloon_OnExplode;
        _baloons[2].id = 2;
        _baloons[2].color = Color.green;
        _baloons[2].setColor();
    }

    private void Balloon_OnExplode(object sender, EventArgs e)
    {
        //Reducimos el numero de globos
        _nBalloons--;
        //Ejecutamos la funcion de re-arrangear
        Baloon b = (Baloon)sender;
        Debug.Log(" * Balloon Exploded: "+b.id);
        Destroy(b.gameObject);
        rearrange(b.id);
    }

    private float comptueX()
    {
        float left = 0f;
        float right = 0f;

        if ((_baloons[0] != null) && (_baloons[1] != null))
        {
            left = _baloons[0].airP;
            right = _baloons[1].airP;
        }
        return right - left;
    }

    private float computeY()
    {
        float speed = 1f;
        if (_baloons[2]!=null)
            speed = _baloons[2].airP;
        return speed;
    }

    private void rearrange(int position)
    {
        if (_nBalloons == 2)
        {
            if (position == 0)
            {
                swap(0, 2); //Si perdimos el globo izquierdo, cambiamos el superior por el izquierdo.
                _baloons[0].moveToNextBalloon(0);
                _baloons[0].id = 0; //Actualizamos el 'id' del objeto
            }
            if (position == 1)
            {
                swap(1, 2); //Si perdimos el globo derecho, cambiamos el superior por el derecho.
                _baloons[1].moveToNextBalloon(1);  //Actualizamos la posicion del objeto
                _baloons[1].id = 1; //Actualizamos el 'id' del objeto
            }

            //Si perdimos el globo superior no hacemos nada
        }
        if (_nBalloons == 1)
        {
            if (position == 0)
            {
                swap(2, 1); //Si perdimos el globo izquierdo, cambiamos el derecho por el superior.
                _baloons[2].moveToNextBalloon(2); //Actualizamos la posicion del objeto
                _baloons[2].id = 2; //Actualizamos el 'id' del objeto
            }
            if (position == 1)
            {
                swap(2, 0); //Si perdimos el globo derecho, cambiamos el izquierdo por el superior.
                _baloons[2].moveToNextBalloon(2);  //Actualizamos la posicion del objeto
                _baloons[2].id = 2; //Actualizamos el 'id' del objeto
            }
        }
    }

    private void swap(int pos1, int pos2)
    {
        Baloon temp = _baloons[pos1];
        _baloons[pos1] = _baloons[pos2];
        _baloons[pos2] = temp;
    }

    private void ScreenWrap()
    {
        _viewp = Camera.main.WorldToViewportPoint(transform.position);
        if (!_isWraping && (_viewp.x > 1 || _viewp.x < 0))
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
            _isWraping = true;
            //_rb.MovePosition(_rb.position + new Vector2(comptueX(), computeY()) * Time.deltaTime * velocity);
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


        if (InputManager.instance.getInput("globo_i").isPressed()) _baloons[0].DeInflate();
        if (InputManager.instance.getInput("globo_c").isPressed()) _baloons[2].DeInflate();
        if (InputManager.instance.getInput("globo_d").isPressed()) _baloons[1].DeInflate();
        
        //Hay que comprobar que siguen "vivos" antes de intentar explotarlos otra vez
        //Cuando un globo explota debido a como tenemos montao todo el tinglado sigue estando las mismas teclas asignadas
        //Habría que hacer null checking de todas formas
        if (InputManager.instance.getInput("globo_i").OnPointerDown()) _baloons[0].Explode();
        if (InputManager.instance.getInput("globo_c").OnPointerDown()) _baloons[2].Explode();
        if (InputManager.instance.getInput("globo_d").OnPointerDown()) _baloons[1].Explode();

    }
}
