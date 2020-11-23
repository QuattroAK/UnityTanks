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

    public void Init(float baseDamage)
    {
        activePoolShots = new List<BulletController>();
        diactivePoolShots = new Stack<BulletController>();

        for (int i = 0; i < countPoolObjects; i++)
        {
            var bulletObject = Instantiate(shotPrefab, shotSpawn.position, shotSpawn.rotation, shotSpawn);
            bulletObject.Init(tankMask, baseDamage);
            bulletObject.gameObject.SetActive(false);
            bulletObject.OnDisabled += BulletDisableHandler;
            diactivePoolShots.Push(bulletObject);
        }
    }

    public BulletController GetBullet()
    {
        BulletController bullet;

        if (diactivePoolShots.Count !=0)
        {
            bullet = diactivePoolShots.Pop();
            activePoolShots.Add(bullet);
        }
        else
        {
            bullet = Instantiate(shotPrefab, shotSpawn.position, shotSpawn.rotation, shotSpawn);
            activePoolShots.Add(bullet);
        }
        return bullet;
    }

    //public BulletController SpawnBullet(Transform parent)
    //{

    //    //var bullet = GetBullet();
    //    //bullet.StartToMove(shotSpawn, parent);
    //    //return bullet;
    //}

    private void BulletDisableHandler(BulletController bullet)
    {
        activePoolShots.Remove(bullet);
        diactivePoolShots.Push(bullet);
    }
}
