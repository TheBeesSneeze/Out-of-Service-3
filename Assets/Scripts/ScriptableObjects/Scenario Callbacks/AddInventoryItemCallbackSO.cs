/*****************************************************************************
// File Name :         AddInventoryItemCallbackSO.cs
// Author :            Kyle Grenier
// Creation Date :     09/25/2021
//
// Brief Description : Adds an item to the player's inventory.
*****************************************************************************/
using UnityEngine;

/// <summary>
/// Adds an item to the Scenario's popup list if a condition is met.
/// </summary>
[CreateAssetMenu(menuName = "FMV Maker/Scenarios/Callbacks/Add Inventory Item", fileName = "New Add Item Callback")]
public class AddInventoryItemCallbackSO : ScenarioCallbackSO
{
    [SerializeField] private Item item;

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="scenario">The FMVScenarioSO which invoked the callback.</param>
    public override void PerformCallback(FMVScenarioSO scenario)
    {
        Inventory.instance.AddItem(item);
    }
}