using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    [SerializeField] private GameObject[] _projectilePrefabs;

    public void ShootBullet(RangeWeaponHandler rangeWeaponHandler, Vector2 startPos, Vector2 dir)
    {
        GameObject origin = _projectilePrefabs[rangeWeaponHandler.BulletIndex];
        GameObject obj = Instantiate(origin, startPos, Quaternion.identity);

        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController.Init(dir, rangeWeaponHandler);
    }
}
