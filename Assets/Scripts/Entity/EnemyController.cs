using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController
{
    private EnemyManager _enemyManager;
    private Transform _target;

    [SerializeField] private float _followRange = 15f;

    public void Init(Transform target)
    {
        _target = target;
    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, _target.position);
    }

    protected Vector2 DirectionToTarget()
    {
        return (_target.position - transform.position).normalized;
    }

    protected override void HandleAction()
    {
        base.HandleAction();

        if (weaponHandler == null || _target == null)
        {
            if(!movementDir.Equals(Vector2.zero)) movementDir = Vector2.zero;
            return;
        }

        float distance = DistanceToTarget();
        Vector2 dir = DirectionToTarget();

        isAttacking = false;

        if (distance <= _followRange)
        {
            lookDir = dir;

            if(distance <= weaponHandler.AttackRange)
            {
                int layerMaskTarget = weaponHandler.target;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, weaponHandler.AttackRange * 1.5f, (1<<LayerMask.NameToLayer("Level") | layerMaskTarget));

                if(hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                {
                    isAttacking = true;
                }

                movementDir = Vector2.zero;
                return;
            }

            movementDir = dir;
        }
    }
}
