using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private BulletsController bulletsController;
    [SerializeField] private Slider aimSlider;
    [SerializeField] private float minLaunchForce;
    [SerializeField] private float maxLaunchForce;
    [SerializeField] private float maxChargeTime;
    [SerializeField] private float baseDamage;
    [SerializeField] private Transform fireTransform;

    private float chargeSpeed;
    private float currentLaunchForece;
    private string fireButton;
    private bool fired;

    public void Init(int playerNumber)
    {
        currentLaunchForece = minLaunchForce;
        aimSlider.value = minLaunchForce;
        fireButton = $"{"Fire" + playerNumber}";
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime; // ? путь на время
        bulletsController.Init(baseDamage);
    }

    public void Refresh()
    {
        aimSlider.value = minLaunchForce;

        if (currentLaunchForece >= maxLaunchForce && !fired)
        {
            currentLaunchForece = maxLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(fireButton))
        {
            fired = false;
            currentLaunchForece = minLaunchForce;
        }
        else if (Input.GetButton(fireButton) && !fired)
        {
            currentLaunchForece += chargeSpeed * Time.fixedDeltaTime;
            aimSlider.value = currentLaunchForece;
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
            bullet.gameObject.SetActive(true);
        }
        bullet.RbBullet.velocity = currentLaunchForece * fireTransform.forward;
        currentLaunchForece = minLaunchForce;
    }
}
