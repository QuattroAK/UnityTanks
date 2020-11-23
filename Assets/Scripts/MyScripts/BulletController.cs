using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletController : MonoBehaviour
{
    [Header("Component links")]
    [SerializeField] private float explosionForce;// где лучше их оставить?
    [SerializeField] private float explosionRadius;
    [SerializeField] private Rigidbody rbBullet;

    public Rigidbody RbBullet
    {
        get
        {
            return rbBullet;
        }
    }

    private LayerMask tankMask;
    private float maxDamage;

    public event Action<BulletController> OnDisabled;
    
    public void Init(LayerMask tankMask, float maxDamage)
    {
        this.tankMask = tankMask;
        this.maxDamage = maxDamage;
    }

    public void StartToMove(Transform spawnTransform, Transform parentObject)
    {
        transform.parent = parentObject;
        transform.position = spawnTransform.position;
        transform.rotation = spawnTransform.rotation;
    }

    private void Disable()
    {
        gameObject.SetActive(false);
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
        }
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
