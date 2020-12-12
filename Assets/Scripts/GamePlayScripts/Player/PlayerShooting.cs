using UnityEngine.UI;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Components links")]
    [SerializeField] private BulletsController bulletsController;
    [SerializeField] private Transform fireTransform;
    [SerializeField] private Slider aimSlider;

    [Header("Shooting parameters")]
    [SerializeField] private float minLaunchForce;
    [SerializeField] private float maxLaunchForce;
    [SerializeField] private float maxChargeTime;
    [SerializeField] private float baseDamage;

    private float currentLaunchForce;
    private float chargeSpeed;
    private string fireButton;
    private bool fired;

    private PlayerController playerController;

    public void Init(PlayerController playerController, int playerNumber, Transform parentBullet)
    {
        this.playerController = playerController;
        currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;
        fireButton = $"Fire{playerNumber}";
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
        bulletsController.Init(baseDamage, parentBullet);
    }

    public void Refresh()
    {
        aimSlider.value = minLaunchForce;

        if (currentLaunchForce >= maxLaunchForce && !fired)
        {
            currentLaunchForce = maxLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(fireButton))
        {
            playerController.PlayerSoundController.PlaySound(PlayerAudioType.ShotCharging);
            fired = false;
            currentLaunchForce = minLaunchForce;
        }
        else if (Input.GetButton(fireButton) && !fired)
        {
            currentLaunchForce += chargeSpeed * Time.deltaTime;
            aimSlider.value = currentLaunchForce;
        }
        else if (Input.GetButtonUp(fireButton) && !fired)
        {
            Fire();
        }
    }

    private void Fire()
    {
        fired = true;
        var bullet = bulletsController.GetBullet();
        if (bullet != null)
        {
            bullet.transform.position = fireTransform.position;
            bullet.transform.rotation = fireTransform.rotation;
            bullet.gameObject.SetActive(true);
            bullet.SetVelocity(currentLaunchForce * fireTransform.forward);
            currentLaunchForce = minLaunchForce;
            playerController.PlayerSoundController.PlaySound(PlayerAudioType.TankShot);
        }
    }
}
