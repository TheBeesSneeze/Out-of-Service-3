/*****************************************************************************
// File Name :         AddPopupCallbackSO.cs
// Author :            Kyle Grenier
// Creation Date :     09/25/2021
//
// Brief Description : Adds an item to the Scenario's popup list if a condition is met.
*****************************************************************************/
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Adds an item to the Scenario's popup list if a condition is met.
/// </summary>
[CreateAssetMenu(menuName = "FMV Maker/Scenarios/Callbacks/Add Popup", fileName = "New Add Popup Callback")]
public class AddPopupCallbackSO : ScenarioCallbackSO
{
    [Tooltip("The popup to add to the Scenario.")]
    [SerializeField] FMVTimedObjectPopup popup;

    /// <summary>
    /// An enum of conditions that must be met to add a popup.
    /// </summary>
    protected enum AddPopupCondition { NONE, REQUIRE_INVENTORY_ITEM };
    [Tooltip("The condition that must be met for the popup to be added.")]
    [SerializeField] protected AddPopupCondition condition = AddPopupCondition.NONE;

    // REQUIRE_INVENTORY_ITEM Fields
    [ShowIf("condition", AddPopupCondition.REQUIRE_INVENTORY_ITEM)]
    [BoxGroup("Required Item Condition")]
    [Tooltip("The item required to be present in the inventory to add the popup to the list.")]
    [AssetSelector]
    [SerializeField] private Item requiredItem;


    /// <summary>
    /// Adds a popup to the Scenario based on the set condition.
    /// </summary>
    /// <param name="scenario">The Scenario which invoked this callback.</param>
    public override void PerformCallback(FMVScenarioSO scenario)
    {
        switch (condition)
        {
            // Add the popup to the list, no requirements.
            case AddPopupCondition.NONE:
                scenario.AddPopup(popup);
                break;
            // Add the popup to the list if the player has a
            // certain item in their inventory.
            case AddPopupCondition.REQUIRE_INVENTORY_ITEM:
                if (Inventory.instance.HasItem(requiredItem))
                {
                    Debug.Log("Adding new popup since it's in the inventory.");
                    scenario.AddPopup(popup);
                }
                break;
        }
    }
}