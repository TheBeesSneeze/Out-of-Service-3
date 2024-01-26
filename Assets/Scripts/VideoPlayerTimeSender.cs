/*****************************************************************************
// File Name :         VideoPlayerTimeSender.cs
// Author :            Kyle Grenier
// Creation Date :     09/18/2021
//
// Brief Description : Saves the time of the VideoPlayer whenever the attached GameObject gets disabled.
*****************************************************************************/
#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Video;
using Sirenix.OdinInspector;

[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerTimeSender : MonoBehaviour
{
    [Tooltip("The channel to receive the time elapsed from.")]
    [SerializeField] private DoubleChannelSO timeElapsedChannel;

    [Tooltip("The elapsed time in the current video.")]
    [SerializeField] [ReadOnly] private double elapsedTime;

    private void OnEnable()
    {
        timeElapsedChannel.OnEventRaised += UpdateTime;
    }

    private void OnDisable()
    {
        timeElapsedChannel.OnEventRaised -= UpdateTime;
    }

    /// <summary>
    /// Raise the event whenever the attached application quits.
    /// </summary>
    private void OnApplicationQuit()
    {
        print("Setting Pref to " + elapsedTime);
        PlayerPrefs.SetFloat("Time Elapsed", (float) elapsedTime);
    }

    /// <summary>
    /// Updates the time elapsed.
    /// </summary>
    /// <param name="elapsedTime">The time elapsed.</param>
    void UpdateTime(double elapsedTime)
    {
        this.elapsedTime = elapsedTime;
    }
}
#endif