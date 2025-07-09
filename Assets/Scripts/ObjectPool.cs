using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private T _prefab;
    private Queue<T> _objects = new Queue<T>();
    private Transform _parent;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            _objects.Enqueue(obj);
        }
    }

    public T Get(Transform newParent)
    {
        T obj = _objects.Count > 0 ? _objects.Dequeue() : Object.Instantiate(_prefab, _parent);
        obj.transform.SetParent(newParent, false);
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(_parent, false);
        _objects.Enqueue(obj);
    }
}