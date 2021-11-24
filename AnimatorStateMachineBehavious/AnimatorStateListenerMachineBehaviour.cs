using UnityEngine;
using System;
using System.Collections.Generic;

public class AnimatorStateListenerMachineBehaviour : StateMachineBehaviour
{
    private Dictionary<int, Action<AnimatorStateInfo>> enterListeners;
    private Dictionary<int, Action<AnimatorStateInfo>> exitListeners;
    private Dictionary<int, Action<AnimatorStateInfo>> updateListeners;

    public enum Transition
    {
        Enter,
        Exit,
        Update
    }

    public void AddListener(string stateName, Transition transition, Action<AnimatorStateInfo> callback)
    {
        int stateHash = Animator.StringToHash(stateName);
        
        switch (transition)
        {
            case Transition.Enter:
                RegisterCallback(ref enterListeners, stateHash, callback);
                break;
            case Transition.Exit:
                RegisterCallback(ref exitListeners, stateHash, callback);
                break;
            case Transition.Update:
                RegisterCallback(ref updateListeners, stateHash, callback);
                break;
        }
    }

    public void RemoveListener(string stateName, Transition transition, Action<AnimatorStateInfo> callback)
    {
        int stateHash = Animator.StringToHash(stateName);
        
        switch (transition)
        {
            case Transition.Enter:
                UnregisterCallback(ref enterListeners, stateHash, callback);
                break;
            case Transition.Exit:
                UnregisterCallback(ref exitListeners, stateHash, callback);
                break;
            case Transition.Update:
                UnregisterCallback(ref updateListeners, stateHash, callback);
                break;
        }
    }
    
    private void UnregisterCallback(ref Dictionary<int, Action<AnimatorStateInfo>> listenersData, int stateHash,
        Action<AnimatorStateInfo> callback)
    {
        if (listenersData.ContainsKey(stateHash))
        {
            listenersData[stateHash] -= callback;
        }
    }

    private void RegisterCallback(ref Dictionary<int, Action<AnimatorStateInfo>> listenersData, int stateHash,
        Action<AnimatorStateInfo> callback)
    {
        if (listenersData.ContainsKey(stateHash))
        {
            listenersData[stateHash] += callback;
        }
        else
        {
            listenersData.Add(stateHash, callback);
        }
    }
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enterListeners.ContainsKey(stateInfo.shortNameHash))
        {
            enterListeners[stateInfo.shortNameHash]?.Invoke(stateInfo);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (exitListeners.ContainsKey(stateInfo.shortNameHash))
        {
            exitListeners[stateInfo.shortNameHash]?.Invoke(stateInfo);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateListeners.ContainsKey(stateInfo.shortNameHash))
        {
            updateListeners[stateInfo.shortNameHash]?.Invoke(stateInfo);
        }
    }
}