using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask _levelCollisionLayer;
    private RangeWeaponHandler _rangeWeaponHandler;

    private float _curDuration;
    private Vector2 dir;
    private bool isReady;
    private Transform _pivot;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    public bool fxOnDestroy = true;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _pivot = transform.GetChild(0);
    }

    private void Update()
    {
        if (!isReady) return;

        _curDuration += Time.deltaTime;

        if(_curDuration > _rangeWeaponHandler.Duration)
        {
            DestroyProjectile(transform.position, false);
        }

        _rb.velocity = dir * _rangeWeaponHandler.Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_levelCollisionLayer.value == (_levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - dir * 0.2f, fxOnDestroy);
        }
        else if(_rangeWeaponHandler.target.value == (_rangeWeaponHandler.target.value | (1 << collision.gameObject.layer)))
        {
            ResourceController resourceController = collision.GetComponent<ResourceController>();
            if (resourceController != null)
            {
                resourceController.ChangeHealth(-_rangeWeaponHandler.Power);
                if (_rangeWeaponHandler.IsOnKnockBack)
                {
                    BaseController controller = collision.GetComponent<BaseController>();
                    if(controller != null)
                    {
                        controller.ApplyKnockback(transform, _rangeWeaponHandler.KnockbackPower, _rangeWeaponHandler.KnockbackTime);
                    }
                }
            }

            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestroy);
        }
    }

    public void Init(Vector2 dir, RangeWeaponHandler weaponHandler)
    {
        _rangeWeaponHandler = weaponHandler;

        this.dir = dir;
        _curDuration = 0;
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        _spriteRenderer.color = weaponHandler.Projectilecolor;

        transform.right = this.dir;

        if(dir.x < 0)
        {
            _pivot.localRotation = Quaternion.Euler(180, 0, 0);
        }
        else
        {
            _pivot.localRotation = Quaternion.Euler(0, 0, 0);
        }
        isReady = true;
    }

    private void DestroyProjectile(Vector3 pos, bool createFx)
    {
        if (createFx)
        {
            ProjectileManager.Instance.CreateImpactParticleAtPosition(pos, _rangeWeaponHandler);
        }

        Destroy(this.gameObject);
    }
}
