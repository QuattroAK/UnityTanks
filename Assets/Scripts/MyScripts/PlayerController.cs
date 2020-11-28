using UnityEngine;
using System;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerShooting))]
[RequireComponent(typeof(PlayerUIController))]
[RequireComponent(typeof(PlayerSoundController))]
public class PlayerController : MonoBehaviour
{
    public event Action OnDamage;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private PlayerUIController playerUIController;
    [SerializeField] private PlayerSoundController playerSoundController;

    #region Properties

    public PlayerController playerController
    {
        get
        {
            return this;
        }
    }

    public float Currenthealth
    {
        get
        {
            return playerHealth.CurrentHealth;
        }
    }

    public float StartingHealth
    {
        get
        {
            return playerHealth.StartingHealth;
        }
    }

    public PlayerSoundController PlayerSoundController
    {
        get
        {
            return playerSoundController;
        }
    }

    #endregion

    public void Init(int playerNumber, Transform parentBullet)
    {
        playerMovement.Init(this, playerNumber);
        playerShooting.Init(this, playerNumber, parentBullet);
        playerHealth.Init(this, OnDamageHandler);
        playerUIController.Init(this);
        playerSoundController.Init();
    }

    public void RefreshFixed()
    {
        playerMovement.Refresh();
    }

    public void Refresh()
    {
        playerShooting.Refresh();
    }

    private void OnDamageHandler()
    {
        playerUIController.UpdateHealth();
        OnDamage?.Invoke();
    }
}