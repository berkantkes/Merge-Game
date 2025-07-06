using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private Dictionary<Type, object> poolDictionary = new Dictionary<Type, object>();

    public void CreatePool<T>(T prefab, int size) where T : Component
    {
        Type type = typeof(T);

        if (!poolDictionary.ContainsKey(type))
        {
            ObjectPool<T> newPool = new ObjectPool<T>(prefab, size, transform);
            poolDictionary[type] = newPool;
        }
    }

    public T Get<T>(Transform parent) where T : Component
    {
        if (poolDictionary.TryGetValue(typeof(T), out object poolObj))
        {
            return (poolObj as ObjectPool<T>).Get(parent);
        }

        Debug.LogError($"No pool exists for type {typeof(T)}. Call CreatePool first.");
        return null;
    }

    public void Return<T>(T obj) where T : Component
    {
        if (poolDictionary.TryGetValue(typeof(T), out object poolObj))
        {
            (poolObj as ObjectPool<T>).ReturnToPool(obj);
        }
        else
        {
            Debug.LogError($"No pool exists for type {typeof(T)}. Cannot return object.");
        }
    }
}