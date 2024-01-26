/*****************************************************************************
// File Name :         BoolChannelSO.cs
// Author :            Kyle Grenier
// Creation Date :     09/29/2021
//
// Brief Description : A channel that carries a boolean value.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Events;

public class BoolChannelSO : MonoBehaviour
{
    public UnityAction<bool> OnEventRaised;

    public void RaiseEvent(bool arg)
    {
        OnEventRaised?.Invoke(arg);
    }
}