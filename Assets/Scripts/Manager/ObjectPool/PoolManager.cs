using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public GameObject[] prefabs;
    private Dictionary<int, Queue<GameObject>> pools = new Dictionary<int, Queue<GameObject>>();

    public override void Awake()
    {
        base.Awake();

        for(int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new Queue<GameObject>();
        }
    }

    public GameObject GetObject(int prefabIndex, Vector3 poition, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefabIndex))
        {
            Debug.Log($"프리팹 인덱스 {prefabIndex}에 대한 풀이 존재하지 않습니다.");
            return null;
        }

        GameObject go;
        if (pools[prefabIndex].Count > 0)
        {
            go = pools[prefabIndex].Dequeue();
        }
        else
        {
            go = Instantiate(prefabs[prefabIndex]);
            go.GetComponent<IPoolable>()?.Init(o => ReturnObject(prefabIndex, o));
        }

        go.transform.SetPositionAndRotation(poition, rotation);
        go.SetActive(true);
        go.GetComponent<IPoolable>()?.OnSpawn();
        return go;
    }

    public void ReturnObject(int prefabIndex, GameObject go)
    {
        if (!pools.ContainsKey(prefabIndex))
        {
            Destroy(go);
            return;
        }

        go.SetActive(false);
        pools[prefabIndex].Enqueue(go);
    }
}
