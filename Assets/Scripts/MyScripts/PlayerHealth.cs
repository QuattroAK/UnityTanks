using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth = 100;
    //[SerializeField] private GameObject explozionPrefab;
    [SerializeField] private ParticleSystem explosionParticles;

    private event Action OnDamage;
    private float currentHealth;
    private bool Dead;
    private PlayerController playerController;

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

    public void Init(PlayerController playerController, Action OnDamage)
    {
        this.playerController = playerController;
        this.OnDamage += OnDamage;
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
            playerController.PlayerSoundController.PlaySound(PlayerAudioType.TankDeath);
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Dead = true;
        explosionParticles.transform.position = transform.position;
        explosionParticles.gameObject.SetActive(false);
        explosionParticles.Play();
        gameObject.SetActive(false);
    }
}