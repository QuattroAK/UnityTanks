using System.Collections.Generic;
using UnityEngine;

public class BulletsController : MonoBehaviour
{
    [Header("Bullet parameters")]
    [SerializeField] private BulletController shotPrefab;
    [SerializeField] private Transform shotSpawn;
    [SerializeField] private int countPoolObjects;
    [SerializeField] private LayerMask tankMask;

    private List<BulletController> activePoolShots;
    private Stack<BulletController> diactivePoolShots;
    private float baseDamage;

    public void Init(float baseDamage)
    {
        this.baseDamage = baseDamage;
        activePoolShots = new List<BulletController>();
        diactivePoolShots = new Stack<BulletController>();

        for (int i = 0; i < countPoolObjects; i++)
        {
            CreateNewBullet();
        }
    }

    public BulletController GetBullet()
    {
        BulletController bullet;

        if (diactivePoolShots.Count != 0)
        {
            bullet = diactivePoolShots.Pop();
            activePoolShots.Add(bullet);
        }
        else
        {
            bullet = CreateNewBullet();
            activePoolShots.Add(bullet);
        }
        return bullet;
    }

    private BulletController CreateNewBullet()
    {
        var bulletObject = Instantiate(shotPrefab, shotSpawn.position, shotSpawn.rotation, shotSpawn);
        bulletObject.Init(shotSpawn, tankMask, baseDamage);
        bulletObject.gameObject.SetActive(false);
        bulletObject.OnDisabled += BulletDisableHandler;
        diactivePoolShots.Push(bulletObject);

        return bulletObject;
    }

    private void BulletDisableHandler(BulletController bullet)
    {
        activePoolShots.Remove(bullet);
        diactivePoolShots.Push(bullet);
    }
}