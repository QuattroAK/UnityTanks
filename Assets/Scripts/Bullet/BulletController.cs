using UnityEngine;
using DG.Tweening;
using System;

public class BulletController : MonoBehaviour
{
    [Header("Component links")]
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private Rigidbody rbBullet;
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private AudioSource explosionAudio;

    private Transform startTransform;
    private LayerMask tankMask;
    private float maxDamage;

    public event Action<BulletController> OnDisabled;

    public void Init(Transform startTransform, LayerMask tankMask, float maxDamage)
    {
        this.startTransform = startTransform;
        this.tankMask = tankMask;
        this.maxDamage = maxDamage;
    }

    public void SetVelocity(Vector3 value)
    {
        rbBullet.velocity = value;
    }

    private void ShellExplosion()
    {
        explosionParticles.transform.parent = gameObject.transform.parent;
        explosionParticles.transform.position = gameObject.transform.position;
        explosionParticles.transform.rotation = gameObject.transform.rotation;
        explosionParticles.gameObject.SetActive(true);
        explosionParticles.Play();
    }

    private void DisableExplosion()
    {
        explosionParticles.gameObject.SetActive(false);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
        rbBullet.velocity = Vector3.zero;
        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;
        OnDisabled?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, tankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRigidbody)
            {
                continue;
            }

            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            PlayerHealth playerHealth = targetRigidbody.GetComponent<PlayerHealth>();

            if (!playerHealth)
            {
                continue;
            }

            float damage = CalculateDamage(targetRigidbody.position);
            playerHealth.TakeDamage(damage);
            ShellExplosion();
            DOVirtual.DelayedCall(explosionParticles.main.duration, DisableExplosion);
            explosionAudio.Play();
            Disable();
            return;
        }
        ShellExplosion();
        DOVirtual.DelayedCall(explosionParticles.main.duration, DisableExplosion);
        explosionAudio.Play();
        Disable();
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        Vector3 explosionToTarget = targetPosition - transform.position;
        float explosionDistance = explosionToTarget.magnitude;
        float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;
        float damage = relativeDistance * maxDamage;
        damage = Mathf.Max(0.0f, damage);
        return damage;
    }
}
