/*****************************************************************************
// File Name :         LoadTitleCallbackSO.cs
// Author :            Kyle Grenier
// Creation Date :     09/22/2021
//
// Brief Description : A callback which loads to the title screen.
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "FMV Maker/Scenarios/Callbacks/Load Title", fileName = "LoadTitleCallback")]
public class LoadTitleCallbackSO : ScenarioCallbackSO
{
    /// <summary>
    /// Loads the MainMenu scene.
    /// </summary>
    /// <param name="scenario">The FMVScenarioSO from which this callback was invoked.</param>
    public override void PerformCallback(FMVScenarioSO scenario)
    {
        SceneManager.LoadScene("MainMenu");
    }
}
