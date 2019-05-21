using System;
using System.Collections;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public float airQuantity = 1f;
    public float inflateSpeed = 0.1f;
    public float deinflateCooldown = 1f;
    public int balloonId;

    bool _hasToMove = false;
    int _nextPositionIndex;
    float _moveSpeed = 1f;
    float _deinflateCounter = 0f;
    bool _deinflating = false;
    Animator _anim;
    SpriteRenderer _sr;

    public event EventHandler OnExplode;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _deinflateCounter = deinflateCooldown;
        _anim.Play("Idle");
    }
    public void StartDeinflate(object sender, EventArgs e)
    {
        _deinflating = true;
    }
    public void StopDeinflate(object sender, EventArgs e)
    {
        _deinflating = false;
    }
    public void Explode(object sender, EventArgs e)
    {
        StartCoroutine(ExplodeCorroutine());
    }
    public void Inflate()
    {
        airQuantity = Mathf.Clamp((airQuantity + inflateSpeed * Time.deltaTime), 0f, 1f);
    }
    public void DeInflate()
    {
        airQuantity = Mathf.Clamp((airQuantity - inflateSpeed * Time.deltaTime), 0f, 2f);
        _deinflateCounter = 0f;
    }
    public void moveToNextBalloon(int next)
    {
        _hasToMove = true;
        _nextPositionIndex = next;
    }

    public void setColor (Color c)
    {
        _sr.color = c;
    }
    public Color getColor()
    {
        return _sr.color;
    }

    private IEnumerator ExplodeCorroutine()
    {
        _anim.Play("Explode");
        Player.instance.addForce(getMyForce());
        yield return new WaitForSeconds(0.5f);
        CameraManager.singleton.ShakeX(0.1f);
        AudioManager.singleton.Play("sfxBalloon");
        yield return new WaitForSeconds(0.1f);
        airQuantity = 0;
        OnExplode(this, EventArgs.Empty);
    }

    private Vector2 getMyForce()
    {
        switch (balloonId)
        {
            case 0: return new Vector2(1, 1);
            case 1: return new Vector2(-1, 1);
            case 2: return new Vector2(0, 1);
        }
        return new Vector2(0, 0);
    }

    private void FixedUpdate()
    {
        //Cambiar por Animator
        transform.localScale = new Vector3(transform.localScale.x, airQuantity, transform.localScale.z);

        if (_deinflating)
        {
            DeInflate();
        }

        if (airQuantity < 1f && _deinflateCounter > deinflateCooldown)
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
