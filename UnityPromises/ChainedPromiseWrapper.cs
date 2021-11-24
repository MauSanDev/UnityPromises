using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Contains a chain of Promises and continues when the chain finish.
/// </summary>
public class ChainedPromiseWrapper : UnityPromise
{
    private UnityPromise mainPromise = null;
    private Coroutine promiseCoroutine = null;
    private Action OnStopped = null;
    private MonoBehaviour owner = null;

    public ChainedPromiseWrapper(MonoBehaviour monoBehaviour)
    {
        owner = monoBehaviour;
    }

    public void Chain(UnityPromise promise)
    {
        if (mainPromise == null)
        {
            mainPromise = promise;
        }
        else
        {
            mainPromise.EndPromise.Then(promise);
        }
    }

    protected override void Execute()
    {
        promiseCoroutine = owner.StartCoroutine(ExecutionRoutine());
    }

    protected override void Stop()
    {
        if (promiseCoroutine != null)
        {
            owner.StopCoroutine(promiseCoroutine);
            mainPromise.StopAll();
        }
    }

    private IEnumerator ExecutionRoutine()
    {
        bool flag = false;

        mainPromise.EndPromise.Finally(() => flag = true);

        yield return new WaitUntil(() => flag);

        Resolve();
    }
}
