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

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleAction();
        Rotate(lookDir);
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
        dir *= 5;
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
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power;
    }
}
