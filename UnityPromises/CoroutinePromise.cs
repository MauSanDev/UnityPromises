using System.Collections;
using UnityEngine;

/// <summary>
/// Executes a Coroutine and continue when it's finished. Should have an owner MonoBehaviour to execute the routine.
/// </summary>
public class CoroutinePromise : UnityPromise
{
    private Coroutine promiseCoroutine;
    private IEnumerator promiseBehaviour;
    private MonoBehaviour owner = null;
    
    public CoroutinePromise(IEnumerator behaviour, MonoBehaviour monoBehaviour)
    {
        owner = monoBehaviour;
        promiseBehaviour = behaviour;
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
        }
    }

    private IEnumerator ExecutionRoutine()
    {
        yield return promiseBehaviour;
        Resolve();
    }
}
