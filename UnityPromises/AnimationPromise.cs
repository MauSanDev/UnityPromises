using UnityEngine;
using System;

/// <summary>
/// Wait for a given Animator to arrive to a State and continues.
/// </summary>
public class AnimationPromise : UnityPromise
{
    private string stateName;
    private AnimatorStateListenerMachineBehaviour.Transition toListen;
    private AnimatorStateListenerMachineBehaviour stateListener = null;

    public AnimationPromise(Animator animator, string stateName, AnimatorStateListenerMachineBehaviour.Transition toListen)
    {
        stateListener = animator.GetBehaviour<AnimatorStateListenerMachineBehaviour>();

        if (stateListener == null)
        {
            throw new Exception("AnimatorPromise depends on an AnimatorStateListener machine behaviour. Add it to the desired animator to use the promise.");
        }
        
        this.stateName = stateName;
        this.toListen = toListen;
    }
    
    private void OnStateExecuted(AnimatorStateInfo stateInfo)
    {
        RemoveListener();
        Resolve();
    }

    private void AddListener() => stateListener.AddListener(stateName, toListen, OnStateExecuted);
    private void RemoveListener() => stateListener.RemoveListener(stateName, toListen, OnStateExecuted);

    protected override void Execute() => AddListener();

    protected override void Stop() => RemoveListener();
}
