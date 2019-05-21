using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 3f;
    public float moveSpeed = 5f;

    bool _hasToMove;

    private void Start()
    {
        moveSpeed = Random.Range(moveSpeed - 2, moveSpeed + 2); 
    }
    private void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + Time.deltaTime * rotationSpeed);

        if (_hasToMove)
        {
            transform.position = Vector3.Lerp(transform.position, Player.instance.transform.position, moveSpeed * Time.deltaTime);
            moveSpeed += Time.deltaTime;
        }

        if (Vector3.Distance(transform.position, Player.instance.transform.position) < 1.5)
        {
            Destroy(gameObject);
        }

    }

    public void MoveToPlayer()
    {
        _hasToMove = true;
    }
}
