/*****************************************************************************
// File Name :         FMVScenarioProgressorData.cs
// Author :            Kyle Grenier
// Creation Date :     09/16/2021
//
// Brief Description : A SO that holds data about the scenario to progress to.
*****************************************************************************/
using Sirenix.OdinInspector;
using UnityEngine;

//[CreateAssetMenu(menuName = "FMV Maker/Scenario Progressor Data", fileName = "New Scenario Progressor Data")]
public class FMVScenarioProgressorData : ScriptableObject
{
    [Tooltip("The scenario to lead into.")]
    [SerializeField] private FMVScenarioSO nextScenario;

    [Tooltip("The channel to raise an event to to signal a change in scenarios.")]
    [HideIf("IsSet")]
    [SerializeField] private FMVScenarioChannelSO scenarioProgressorChannel;
    
    /// <summary>
    /// Requests to progress to the next scenario provided.
    /// </summary>
    public void RequestScenarioProgression()
    {
        scenarioProgressorChannel.RaiseEvent(nextScenario);
    }

    private bool IsSet()
    {
        return scenarioProgressorChannel != null;
    }
}
