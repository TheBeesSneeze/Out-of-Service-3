/*****************************************************************************
// File Name :         FMVScenarioChannelSO.cs
// Author :            Kyle Grenier
// Creation Date :     09/16/2021
//
// Brief Description : A channel that receives subscribes and broadcasts a FMVScenarioSO event.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Channels/FMVScenarioSO Channel", fileName = "New FMVScenarioSO Channel")]
public class FMVScenarioChannelSO : ScriptableObject
{
    public UnityAction<FMVScenarioSO> OnEventRaised;

    public void RaiseEvent(FMVScenarioSO scenario)
    {
        OnEventRaised?.Invoke(scenario);
    }
}