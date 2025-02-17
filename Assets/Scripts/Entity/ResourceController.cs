using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0.5f;

    private BaseController baseController;
    private StatHandler statHandler;
    private AnimationHandler animationHandler;

    private float timeSinceLastChange = float.MaxValue;

    public float CurHealth {  get; private set; }
    public float MaxHealth => statHandler.Health;

    private void Awake()
    {
        baseController = GetComponent<BaseController>();
        statHandler = GetComponent<StatHandler>();
        animationHandler = GetComponent<AnimationHandler>();
    }

    private void Start()
    {
        CurHealth = statHandler.Health;
    }

    private void Update()
    {
        if (timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            if(timeSinceLastChange >= healthChangeDelay)
            {
                animationHandler.InvinciblilityEnd();
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        if(change == 0 || timeSinceLastChange <  healthChangeDelay)
        {
            return false;
        }

        timeSinceLastChange = 0;
        CurHealth += change;
        CurHealth = CurHealth > MaxHealth ? MaxHealth : CurHealth;
        CurHealth = CurHealth < 0 ? 0 : CurHealth;

        if(change < 0)
        {
            animationHandler.Hit();
        }

        if(CurHealth <= 0)
        {
            Death();
        }

        return true;
    }

    private void Death()
    {
        baseController.Death();
    }
}
