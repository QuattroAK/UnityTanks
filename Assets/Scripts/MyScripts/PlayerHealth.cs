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

    public void Init(Action OnDamage)
    {
        this.OnDamage += OnDamage;
        currentHealth = startingHealth;
        Dead = false;
        explosionParticles.gameObject.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        OnDamage?.Invoke();

        if (currentHealth <=0.0f && !Dead)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Dead = true;
        explosionParticles.transform.position = transform.position;
        explosionParticles.gameObject.SetActive(false);
        explosionParticles.Play();
        // PlayAudio
        gameObject.SetActive(false);
    }
}
