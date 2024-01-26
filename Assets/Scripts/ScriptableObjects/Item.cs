/*****************************************************************************
// File Name :         Item.cs
// Author :            Kyle Grenier
// Creation Date :     09/25/2021
//
// Brief Description : An inventory item that holds a name.
*****************************************************************************/
using UnityEngine;

/// <summary>
/// An inventory item that holds a name.
/// </summary>
[CreateAssetMenu(menuName = "FMV Maker/Inventory Item", fileName = "New Inventory Item")]
public class Item : ScriptableObject
{
    [Tooltip("The item's name.")]
    [SerializeField] private string itemName;

    /// <summary>
    /// The item's name.
    /// </summary>
    public string ItemName
    {
        get
        {
            return itemName;
        }
    }
}