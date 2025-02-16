using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    [SerializeField] private GameObject[] _projectilePrefabs;

    [SerializeField] private ParticleSystem _impactParticleSystem;

    public void ShootBullet(RangeWeaponHandler rangeWeaponHandler, Vector2 startPos, Vector2 dir)
    {
        GameObject origin = _projectilePrefabs[rangeWeaponHandler.BulletIndex];
        GameObject obj = Instantiate(origin, startPos, Quaternion.identity);

        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController.Init(dir, rangeWeaponHandler);
    }

    public void CreateImpactParticleAtPosition(Vector3 pos, RangeWeaponHandler weaponHandler)
    {
        _impactParticleSystem.transform.position = pos;
        ParticleSystem.EmissionModule em = _impactParticleSystem.emission;
        em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(weaponHandler.BulletSize * 5)));

        ParticleSystem.MainModule mainModule = _impactParticleSystem.main;
        mainModule.startSpeedMultiplier = weaponHandler.BulletSize * 10f;
        _impactParticleSystem.Play();
    }
}
