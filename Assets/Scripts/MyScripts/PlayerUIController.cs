using UnityEngine.UI;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private Slider sliderHealth;
    [SerializeField] private Image fillImageHealth;
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color zeroHealthColor = Color.red;

    private PlayerController playerController;

    public void Init(PlayerController playerController)
    {
        this.playerController = playerController;
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        sliderHealth.value = playerController.Currenthealth / playerController.StartingHealth;
        fillImageHealth.color = Color.Lerp(zeroHealthColor, fullHealthColor, playerController.Currenthealth / playerController.StartingHealth);
    }
}