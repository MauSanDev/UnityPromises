using System;

/// <summary>
/// Execute a Synchronous method.
/// </summary>
public class Promise : UnityPromise
{
    private Action behaviour;
    
    public Promise(Action behaviour)
    {
        this.behaviour = behaviour;
    }
    
    protected override void Execute()
    {
        behaviour?.Invoke();
        Resolve();
    }

    protected override void Stop() { } //don't need stop because the behaviour is synchronous.
}
