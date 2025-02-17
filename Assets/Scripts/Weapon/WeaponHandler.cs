using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Attack Info")]
    [SerializeField] private float delay = 1f;
    public float Delay { get => delay; set => delay = value; }
    [SerializeField] private float weaponSize = 1f;
    public float WeaponSize { get => weaponSize; set => weaponSize = value; }
    [SerializeField] private float power = 1f;
    public float Power { get => power; set => power = value; }
    [SerializeField] private float speed = 1f;
    public float Speed { get => speed; set => speed = value; }
    [SerializeField] private float attackRange = 10f;
    public float AttackRange { get => attackRange; set => attackRange = value; }

    public LayerMask target;

    [Header("Knockback Info")]
    [SerializeField] private bool isOnKnockBack = false;
    public bool IsOnKnockBack { get => isOnKnockBack; set => isOnKnockBack = value; }
    [SerializeField] private float knockbackPower = 0.1f;
    public float KnockbackPower { get => knockbackPower; set => knockbackPower = value; }
    [SerializeField] private float knockbackTime = 0.5f;
    public float KnockbackTime { get => knockbackTime; set => knockbackTime = value; }
    private static readonly int isAttack = Animator.StringToHash("isAttack");
    
    public BaseController Controller { get; private set; }
    private Animator _animator;
    private SpriteRenderer _weaponRenderer;

    public AudioClip attackSoundClip;

    protected virtual void Awake()
    {
        Controller = GetComponentInParent<BaseController>();
        _animator = GetComponentInChildren<Animator>();
        _weaponRenderer = GetComponentInChildren<SpriteRenderer>();

        _animator.speed = 1.0f / delay;
        transform.localScale = Vector3.one * weaponSize;
    }

    protected virtual void Start()
    {

    }

    public virtual void Attack()
    {
        AttackAnimation();

        if(attackSoundClip != null)
        {
            SoundManager.PlayClip(attackSoundClip);
        }
    }

    public void AttackAnimation()
    {
        _animator.SetTrigger(isAttack);
    }

    public virtual void Rotate(bool isLeft)
    {
        _weaponRenderer.flipY = isLeft;
    }
}
