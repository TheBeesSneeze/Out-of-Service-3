/*****************************************************************************
// File Name :         FMVTimedObjectPopup.cs
// Author :            Kyle Grenier
// Creation Date :     09/16/2021
//
// Brief Description : A script that holds a time to wait until the attached game object
                       is instantiated.
*****************************************************************************/
using UnityEngine;

[DisallowMultipleComponent]
public class FMVTimedObjectPopup : MonoBehaviour
{
    [Tooltip("The amount of time to wait before instantiating the GameObject.")]
    [SerializeField] protected float popupTime;
    /// <summary>
    /// The amount of time to wait before instantiating the GameObject.
    /// </summary>
    public float PopupTime
    {
        get
        {
            return popupTime;
        }

        set
        {
            popupTime = value;
        }
    }
}