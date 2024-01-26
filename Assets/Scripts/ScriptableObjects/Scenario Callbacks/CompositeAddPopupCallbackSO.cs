/*****************************************************************************
// File Name :         CompositeAddPopupCallbackSO.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : ADD BRIEF DESCRIPTION OF THE FILE HERE
*****************************************************************************/
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "FMV Maker/Scenarios/Callbacks/Composite Add Popup", fileName = "New Composte Add Popup")]
public class CompositeAddPopupCallbackSO : AddPopupCallbackSO
{
    [Tooltip("The popup to add to the Scenario.")]
    [SerializeField] private FMVTimedObjectPopup popup2;

    [AssetSelector]
    [ShowIf("condition", AddPopupCondition.REQUIRE_INVENTORY_ITEM)]
    [BoxGroup("Required Item Condition")]
    [Tooltip("The item required to be present in the inventory to add the popup to the list.")]
    [SerializeField] private Item requiredItem2;

    public override void PerformCallback(FMVScenarioSO scenario)
    {
        base.PerformCallback(scenario);

        switch (condition)
        {
            // Add the popup to the list, no requirements.
            case AddPopupCondition.NONE:
                scenario.AddPopup(popup2);
                break;
            // Add the popup to the list if the player has a
            // certain item in their inventory.
            case AddPopupCondition.REQUIRE_INVENTORY_ITEM:
                if (Inventory.instance.HasItem(requiredItem2))
                {
                    Debug.Log("Adding new popup since it's in the inventory.");
                    scenario.AddPopup(popup2);
                }
                break;
        }
    }
}