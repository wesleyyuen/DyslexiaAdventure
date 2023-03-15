using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int _poolSize;
    [SerializeField] protected int _maxConcurrent;

    protected List<GameObject> _prefabs;
    protected Queue<GameObject> _pool;

    // prefabs MUST be defined by child class first
    protected virtual void Awake()
    {
        _pool = new Queue<GameObject>();

        for (int i = 0; i < _poolSize; ++i) {
            GameObject obj = Instantiate(_prefabs[Random.Range(0, _prefabs.Count)], transform);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public virtual GameObject SpawnFromPool(Vector3 position, Quaternion rotation)
    {
        GameObject obj = _pool.Dequeue();
  
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        _pool.Enqueue(obj);

        return obj;
    }
}
