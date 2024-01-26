/*****************************************************************************
// File Name :         DoubleChannelSO.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : ADD BRIEF DESCRIPTION OF THE FILE HERE
*****************************************************************************/
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Channels/Double Channel", fileName = "New Double Channel")]
public class DoubleChannelSO : ScriptableObject
{
    public UnityAction<double> OnEventRaised;

    public void RaiseEvent(double d)
    {
        OnEventRaised?.Invoke(d);
    }
}
