using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    public Transform[] pointsToGo;

    int _index = -1;
    Vector3 _nextPosition;
    SpriteRenderer _sr;
    float _lastX = 0;

    private void computeNextPoint()
    {
        if (_index >= (pointsToGo.Length - 1))
        {
            _index = 0;
        }
        else
        {
            _index++;
        }
        _nextPosition = pointsToGo[_index].position;
    }
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        computeNextPoint();
    }
    private void FixedUpdate()
    {
        if (transform.position != _nextPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, _nextPosition, Time.deltaTime*2);
            _sr.flipX= (_lastX <     transform.position.x);
            _lastX = transform.position.x;
        }
        else
        {
            computeNextPoint();
        }
    }

}
