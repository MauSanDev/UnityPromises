using System;

/// <summary>
/// Receives a method that creates a Promise when this one is triggered and executes the gotten Promise.
/// </summary>
public class PatientPromise : UnityPromise
{
    private Func<UnityPromise> promiseGetter = null;
    private UnityPromise createdPromise = null;

    public PatientPromise(Func<UnityPromise> getter)
    {
        promiseGetter = getter;
    }

    protected override void Execute()
    {
        createdPromise = promiseGetter.Invoke();
        createdPromise.Go().Finally(Resolve);
    }

    protected override void Stop()
    {
        createdPromise.StopAll();
    }
}
