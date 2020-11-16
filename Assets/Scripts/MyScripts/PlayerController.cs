using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    //[SerializeField] private PlayerHealth playerHealth;
    //[SerializeField] private PlayerShooting playerShooting;

    public void Init(int playerNumber)
    {
        playerMovement.Init(playerNumber);
    }

    public void Refresh()
    {
        playerMovement.Refresh();
    }
}
