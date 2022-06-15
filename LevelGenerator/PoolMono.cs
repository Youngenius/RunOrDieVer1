using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolMono<T> where T : MonoBehaviour
{
    public T[] prefabs { get; }
    public bool autoExpand { get; set; }
    public Transform container { get; }

    private List<T> pool;
    private List<T> availableElements = new List<T>();

    public PoolMono(T[] prefabs, int count, Transform container)
    {
        this.prefabs = prefabs;
        this.container = container;

        CreatePool(count);
    }

    private void CreatePool(int count)
    {
        pool = new List<T>();

        for (int i = 0; i < count; i++)
        {
            CreateObject(i);
        }
    }

    private T CreateObject(int index ,bool isActiveByDefault = false)
    {
        var createdObject = UnityEngine.Object.Instantiate(prefabs[index], container);

        createdObject.gameObject.SetActive(isActiveByDefault);
        this.pool.Add(createdObject);

        return createdObject;
    }

    public bool HasFreeElement()
    {
        foreach (var mono in pool)
        {
            if (!mono.gameObject.activeInHierarchy)
                return true;
        }

        return false;
    }

    private void InstantiateFreeElements(List<T> pool, out T element)
    {
        T elementToSetActive;
        availableElements = (from elem in pool where !elem.gameObject.activeInHierarchy select elem).ToList();

        elementToSetActive = availableElements[UnityEngine.Random.Range(0, availableElements.Count)];
        elementToSetActive.gameObject.SetActive(true);
        element = elementToSetActive;
    }

    public T GetFreeElement()
    {
        if (this.HasFreeElement())
        {
            InstantiateFreeElements(pool, out T element);
            return element;
        }

        throw new Exception($"There are no free elements of type {typeof(T)}");
    }
}
