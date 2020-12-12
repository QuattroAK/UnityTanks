using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components links")]
    [SerializeField] private Rigidbody rb;

    [Header("Movement paramaters")]
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;

    private string movementAxisName;
    private string turnAxisName;
    private float movementValue;
    private float turnValue;

    private PlayerController playerController;

    public void Init(PlayerController playerController, int playerNumber)
    {
        this.playerController = playerController;

        movementAxisName = $"Vertical{playerNumber}";
        turnAxisName = $"Horizontal{playerNumber}";
    }

    public void Refresh()
    {
        movementValue = Input.GetAxis(movementAxisName);
        turnValue = Input.GetAxis(turnAxisName);

        Move();
        Turn();
        PlayMovementSounds();
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

    private void PlayMovementSounds()
    {
        playerController.PlayerSoundController.PlayEngineAudio(movementValue, turnValue);
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