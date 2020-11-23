using UnityEngine;
using System;

public class BulletController : MonoBehaviour
{
    public event Action<BulletController> OnDisabled;

    [Header("Component links")]
    [SerializeField] private float explosionForce;// где лучше их оставить?
    [SerializeField] private float explosionRadius;
    [SerializeField] private Rigidbody rbBullet;

    private Transform startTransform;

    private LayerMask tankMask;
    private float maxDamage;
    
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
            Disable();
            return;
        }

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
