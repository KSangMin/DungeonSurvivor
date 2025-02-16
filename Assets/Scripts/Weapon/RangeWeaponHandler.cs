using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHandler : WeaponHandler
{
    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPos;

    [SerializeField] private int _bulletIndex;
    public int BulletIndex { get { return _bulletIndex; } }
    [SerializeField] private float _bulletSize;
    public float BulletSize { get { return _bulletSize; } }
    [SerializeField] private float _duration;
    public float Duration { get { return _duration; } }
    [SerializeField] private float _spread;
    public float Spread { get { return _spread; } }
    [SerializeField] private int _projectilePerShot;
    public int ProjectilePerShot { get { return _projectilePerShot; } }
    [SerializeField] private int _projectileAngle;
    public int ProjectileAngle { get { return _projectileAngle; } }
    [SerializeField] private Color _projectileColor;
    public Color Projectilecolor { get { return _projectileColor; } }

    public override void Attack()
    {
        base.Attack();

        float projectileAngleSpace = ProjectileAngle;
        int num = ProjectilePerShot;

        float minAngle = -(num / 2f) * projectileAngleSpace;

        for(int i = 0; i < num; i++)
        {
            float angle = minAngle + projectileAngleSpace * i;
            float randomSpread = Random.Range(-Spread, Spread);
            angle += randomSpread;
            CreateProjectile(Controller.LookDir, angle);
        }
    }

    private void CreateProjectile(Vector2 lookDir, float angle)
    {
        ProjectileManager.Instance.ShootBullet(
            this,
            projectileSpawnPos.position,
            RotateVector2(lookDir, angle)
            );
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
