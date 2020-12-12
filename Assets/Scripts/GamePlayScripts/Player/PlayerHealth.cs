using UnityEngine;
using DG.Tweening;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health settings")]
    [SerializeField] private float startingHealth;
    [Header("Particle components")]
    [SerializeField] private ParticleSystem explosionParticles;

    private float currentHealth;
    private bool Dead;

    private PlayerUIController playerUIController;
    private PlayerController playerController;

    private event Action OnPlayerDie;
    private event Action OnDamage;
    private event Action Reset;

    #region Properties

    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
    }

    public float StartingHealth
    {
        get
        {
            return startingHealth;
        }
    }

    #endregion

    public void Init(PlayerController playerController, Action OnDamage, Action OnPlayerDie, Action Reset, PlayerUIController playerUIController)
    {
        this.playerController = playerController;
        this.playerUIController = playerUIController;
        this.OnDamage += OnDamage;
        this.OnPlayerDie += OnPlayerDie;
        this.Reset += Reset;
        currentHealth = startingHealth;
        Dead = false;
        explosionParticles.gameObject.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        OnDamage?.Invoke();

        if (currentHealth <= 0 && !Dead)
        {
            currentHealth = 0;
            playerController.PlayerSoundController.PlaySound(PlayerAudioType.TankDeath); //TO DO: bug sound
            OnDeath();
        }
    }

    private void OnDeath()
    {
        gameObject.SetActive(false);
        OnPlayerDie.Invoke(); // TO DO: bug double call
        Dead = true;
        PlayParticles();
        DOVirtual.DelayedCall(3f, ResetPlayers);
    }

    private void PlayParticles()
    {
        explosionParticles.transform.parent = transform.parent;
        explosionParticles.transform.position = transform.position;
        explosionParticles.gameObject.SetActive(true);
        explosionParticles.Play();
    }

    private void ResetPlayers()
    {
        Reset.Invoke();
        currentHealth = startingHealth;
        Dead = false;
        playerUIController.UpdateHealth();
    }
}