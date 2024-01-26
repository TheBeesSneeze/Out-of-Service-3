/*****************************************************************************
// File Name :         VideoPlayerTimeHolderSO.cs
// Author :            Kyle Grenier
// Creation Date :     09/17/2021
//
// Brief Description : A SO to hold the time the video player stops at.
*****************************************************************************/
using UnityEngine;

[CreateAssetMenu(menuName = "FMV Maker/VideoPlayer Time Holder", fileName = "New VideoPlayer Time Holder")]
public class VideoPlayerTimeHolderSO : ScriptableObject
{
    /// <summary>
    /// The time the video player last stopped at when it became disabled.
    /// </summary>
    private double savedTime;
    public double SavedTime { get { return savedTime; } }

    [SerializeField] private DoubleChannelSO videoTimeChannel;
}