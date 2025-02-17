using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rb;

    [SerializeField] private SpriteRenderer _characterRenderer;
    [SerializeField] private Transform _weaponPivot;

    protected Vector2 movementDir = Vector2.zero;
    public Vector2 MovementDir { get { return movementDir; } }
    protected Vector2 lookDir = Vector2.zero;
    public Vector2 LookDir { get { return lookDir; } }
    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0f;

    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;

    [SerializeField] public WeaponHandler weaponPrefab;
    protected WeaponHandler weaponHandler;

    protected bool isAttacking;
    private float _timeSinceLastAttack = float.MaxValue;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();
        statHandler = GetComponent<StatHandler>();

        if(weaponPrefab != null)
        {
            weaponHandler = Instantiate(weaponPrefab, _weaponPivot);
        }
        else
        {
            weaponHandler = GetComponentInChildren<WeaponHandler>();
        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleAction();
        Rotate(LookDir);
        HandleAttackDelay();
    }

    protected virtual void FixedUpdate()
    {
        Movement(movementDir);
        if(knockbackDuration > 0f)
        {
            knockbackDuration -= Time.fixedDeltaTime;
        }
    }

    protected virtual void HandleAction()
    {

    }

    public void Movement(Vector2 dir)
    {
        dir *= statHandler.Speed;
        if (knockbackDuration > 0f)
        {
            dir *= 0.2f;
            dir += knockback;
        }
        
        _rb.velocity = dir;
        animationHandler.Move(dir);
    }

    private void Rotate(Vector2 dir)
    {
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90;

        _characterRenderer.flipX = isLeft;

        if (_weaponPivot != null)
        {
            _weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        weaponHandler?.Rotate(isLeft);
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power;
    }

    private void HandleAttackDelay()
    {
        if (weaponHandler is null)
        {
            return;
        }

        if(_timeSinceLastAttack <= weaponHandler.Delay)
        {
            _timeSinceLastAttack += Time.deltaTime;
        }

        if(isAttacking && _timeSinceLastAttack > weaponHandler.Delay)
        {
            _timeSinceLastAttack = 0;
            Attack();
        }
    }

    protected virtual void Attack()
    {
        if (lookDir != Vector2.zero)
        {
            weaponHandler?.Attack();

        }
    }

    public virtual void Death()
    {
        _rb. velocity = Vector3.zero;

        foreach(SpriteRenderer r in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = r.color;
            color.a = 0.3f;
            r.color = color;
        }

        foreach(Behaviour component  in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
}
