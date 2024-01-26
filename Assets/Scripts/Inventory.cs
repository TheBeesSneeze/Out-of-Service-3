/*****************************************************************************
// File Name :         Inventory.cs
// Author :            Kyle Grenier
// Creation Date :     09/25/2021
//
// Brief Description : Holds any items that the player collects during the game.
*****************************************************************************/
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Holds any items that the player collects during the game.
/// </summary>
public class Inventory : MonoBehaviour
{
    [Tooltip("The items being held in this inventory.")]
    [SerializeField] private List<Item> items;

    /// <summary>
    /// The Singleton instance of the Inventory class.
    /// </summary>
    public static Inventory instance;

    /// <summary>
    /// Setting up the Singleton and initializing the list.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Returns true if the inventory contains the item.
    /// </summary>
    /// <param name="item">The item to check the inventory for.</param>
    /// <returns>True if the inventory contains the item.</returns>
    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }
    
    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="item">The item to add to the inventory.</param>
    public void AddItem(Item item)
    {
        items.Add(item);
    }
}