using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int isMoving = Animator.StringToHash("isMove");
    private static readonly int isHit = Animator.StringToHash("isHit");
    
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(isMoving, obj.magnitude > 0.5f);
    }

    public void Hit(Vector2 obj)
    {
        animator.SetBool(isHit, true);
    }

    public void InvinciblilityEnd()
    {
        animator.SetBool(isHit, false);
    }
}
