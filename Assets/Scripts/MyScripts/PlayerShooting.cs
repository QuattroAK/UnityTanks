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
    private float currentLaunchForce;
    private string fireButton;
    private bool fired;

    public void Init(int playerNumber, Transform parentBullet)
    {
        currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;
        fireButton = $"Fire{playerNumber}";
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime; // ? путь на время
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
        }
    }
}
