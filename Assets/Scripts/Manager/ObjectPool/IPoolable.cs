using System;
using UnityEngine;

public interface IPoolable
{
    void Init(Action<GameObject> returnAction);
    void OnSpawn();
    void OnDespawn();
}
