using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private Camera _camera;

    public void Init()
    {
        _camera = Camera.main;
    }

    public override void Death()
    {
        base.Death();
        GameManager.Instance.GameOver();
    }

    void OnMove(InputValue value)
    {
        movementDir = value.Get<Vector2>();
        movementDir = movementDir.normalized;
    }

    void OnLook(InputValue value)
    {
        Vector2 mousePos = value.Get<Vector2>();
        Vector2 worldPos = _camera.ScreenToWorldPoint(mousePos);
        lookDir = (worldPos - (Vector2)transform.position);

        if (lookDir.magnitude < 0.9f)
        {
            lookDir = Vector2.zero;
        }
        else
        {
            lookDir = lookDir.normalized;
        }
    }

    void OnFire(InputValue value)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        isAttacking = value.isPressed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<ItemHandler>(out ItemHandler item))
        {
            if (item.ItemData == null) return;

            UseItem(item.ItemData);
            Destroy(item.gameObject);
        }   
    }

    public void UseItem(ItemData item)
    {
        foreach(StatEntry modifier in item.statModifiers)
        {
            statHandler.ModifyStat(modifier.statType, modifier.baseValue, !item.isTemporary, modifier.baseValue);
        }
    }
}
