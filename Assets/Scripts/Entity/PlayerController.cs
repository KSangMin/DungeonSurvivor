using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    private Camera _camera;

    public void Init()
    {
        _camera = Camera.main;
    }

    protected override void HandleAction()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");
        movementDir = new Vector2(hor, ver).normalized;

        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = _camera.ScreenToWorldPoint(mousePos);
        lookDir = (worldPos - (Vector2)transform.position);

        if(lookDir.magnitude < 0.9f)
        {
            lookDir = Vector2.zero;
        }
        else
        {
            lookDir = lookDir.normalized;
        }

        isAttacking = Input.GetMouseButton(0);
    }
}
