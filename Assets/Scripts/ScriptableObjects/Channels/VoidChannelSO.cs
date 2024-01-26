/*****************************************************************************
// File Name :         VoidChannelSO.cs
// Author :            Kyle Grenier
// Creation Date :     09/22/2021
//
// Brief Description : A channel that accepts subscribers and broadcasts a void event.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Channels/Void Channel", fileName = "New Void Channel")]
public class VoidChannelSO : ScriptableObject
{
    /// <summary>
    /// The UnityAction (event) to subscribe to.
    /// </summary>
    public UnityAction OnEventRaised;

    /// <summary>
    /// Invokes the event if it has any subscribers.
    /// </summary>
    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}