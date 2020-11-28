﻿using UnityEngine;
using System;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerShooting))]
[RequireComponent(typeof(PlayerUIController))]
public class PlayerController : MonoBehaviour
{
    public event Action OnDamage;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private PlayerUIController playerUIController;

    #region Properties

    public PlayerController playerController
    {
        get
        {
            return this; ;
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

    #endregion

    public void Init(int playerNumber, Transform parentBullet)
    {
        playerMovement.Init(playerNumber);
        playerShooting.Init(playerNumber, parentBullet);
        playerHealth.Init(OnDamageHandler);
        playerUIController.Init(this);
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