using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private float speed = 12f;
    private float turnSpeed = 180f;
    private string movementAxisName;
    private string turnAxisName;
    private float movementValue;
    private float turnValue;

    public void Init(int playerNumber)
    {
        movementAxisName = $"Vertical{playerNumber}";
        turnAxisName = $"Horizontal{playerNumber}";
    }

    public void Refresh()
    {
        movementValue = Input.GetAxis(movementAxisName);
        turnValue = Input.GetAxis(turnAxisName);

        Move();
        Turn();
    }

    private void Move()
    {
        Vector3 movement = transform.forward * speed * movementValue * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void Turn()
    {
        float turn = turnValue * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0.0f, turn, 0.0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
        movementValue = 0f;
        turnValue = 0;
    }

    private void OnDisable()
    {
        rb.isKinematic = true;
    }
}