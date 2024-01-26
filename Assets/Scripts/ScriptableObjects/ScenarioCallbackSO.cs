/*****************************************************************************
// File Name :         ScenarioCallbackSO.cs
// Author :            Kyle Grenier
// Creation Date :     09/22/2021
//
// Brief Description : Abstract class that performs scenario callbacks.
*****************************************************************************/
using UnityEngine;

public abstract class ScenarioCallbackSO : ScriptableObject
{
    /// <summary>
    /// Performs the concrete Scenario callback function.
    /// </summary>
    /// <param name="scenario">The FMVScenarioSO from which the callback was invoked.</param>
    public abstract void PerformCallback(FMVScenarioSO scenario);
}