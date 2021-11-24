using System;

public abstract class UnityPromise : IDisposable
{
    private Action OnPromiseCompleted;
    private bool disposed = false;

    public UnityPromise NextPromise { get; private set; }

    public UnityPromise EndPromise
    {
        get
        {
            UnityPromise toReturn = this;
            while (toReturn.NextPromise != null)
            {
                toReturn = toReturn.NextPromise;
            }

            return toReturn;
        }
    }

    public UnityPromise Then(UnityPromise nextPromise)
    {
        NextPromise = nextPromise;
        return nextPromise;
    }

    public void Finally(Action then)
    {
        OnPromiseCompleted = then;
    }

    public UnityPromise Go()
    {
        Execute();
        return this;
    }

    protected abstract void Execute();

    protected abstract void Stop();

    protected void Resolve()
    {
        OnPromiseCompleted?.Invoke();
        NextPromise?.Execute();
    }

    public void StopAll()
    {
        Stop();
        NextPromise?.Dispose();
    }
    
    private void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                StopAll();
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~UnityPromise()
    {
        Dispose(false);
    }
}

