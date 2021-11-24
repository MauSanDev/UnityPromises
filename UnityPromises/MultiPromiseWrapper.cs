using System.Collections;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Register a List of Promises and execute all at the same time. Continues when all Promises finish.
/// </summary>
public class MultiPromiseWrapper : UnityPromise
{
    private Coroutine promiseCoroutine = null;
    private MonoBehaviour owner = null;
    
    private List<UnityPromise> currentPromises = new List<UnityPromise>();

    public MultiPromiseWrapper(MonoBehaviour monoBehaviour)
    {
        owner = monoBehaviour;
    }
    
    public MultiPromiseWrapper(MonoBehaviour monoBehaviour, params UnityPromise[] promises)
    {
        owner = monoBehaviour;
        currentPromises = new List<UnityPromise>(promises);
    }

    public void Add(UnityPromise promise) => currentPromises.Add(promise);
    public void Remove(UnityPromise promise) => currentPromises.Remove(promise);
    public void Clear() => currentPromises.Clear();
    public int Count => currentPromises.Count;

    protected override void Execute()
    {
        promiseCoroutine = owner.StartCoroutine(ExecutionRoutine());
    }

    protected override void Stop()
    {
        if (promiseCoroutine != null)
        {
            owner.StopCoroutine(promiseCoroutine);
            for (int i = 0; i < currentPromises.Count; i++)
            {
                currentPromises[i].StopAll();
            }
        }
    }

    protected IEnumerator ExecutionRoutine()
    {
        if (currentPromises.Count == 0)
        {
            Resolve();
        }
        
        int finishedPromises = 0;

        for (int i = 0; i < currentPromises.Count; i++)
        {
            currentPromises[i].Finally(() => finishedPromises++);
            currentPromises[i].Go();
        }

        yield return new WaitUntil(() => finishedPromises == currentPromises.Count);

        Resolve();
    }
}