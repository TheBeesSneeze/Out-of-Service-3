/*****************************************************************************
// File Name :         FMVManager.cs
// Author :            Kyle Grenier
// Creation Date :     09/16/2021
//
// Brief Description : Controls the flow of the FMV.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using Sirenix.OdinInspector;

public class FMVManager : MonoBehaviour
{
    [Tooltip("The scenario to play.")]
    [Required("A starting scenario is required.")]
    [SerializeField] private FMVScenarioSO currentScenario;

    [Tooltip("The VideoPlayer that plays the current scenario's video.")]
    [Required("A reference to the VideoPlayer is required.")]
    [SerializeField] private VideoPlayer videoParent;

    [FoldoutGroup("Timing Channels")]
    [Tooltip("Channel to broadcast the time elapsed to.")]
    [SerializeField] private DoubleChannelSO timeElapsedChannel;

    [FoldoutGroup("Timing Channels")]
    [Tooltip("Channel to receive calls to progress the scenario from.")]
    [SerializeField] private FMVScenarioChannelSO scenarioProgressorChannel;

    [FoldoutGroup("Timing Channels")]
    [Tooltip("The channel to accept time seek requests from.")]
    [SerializeField] private DoubleChannelSO seekRequestChannel;

    #region -- // Management Tools // --
    //[Button("Clear All Channels", ButtonSizes.Gigantic)]
    //[FoldoutGroup("Management Tools")]
    //[HorizontalGroup("Management Tools/Horizontal")]
    //[BoxGroup("Management Tools/Horizontal/Channel Tools")]
    //// Sets all channels' events to null.
    //private void ClearAllChannels()
    //{
    //    timeElapsedChannel.OnEventRaised = null;
    //    scenarioProgressorChannel.OnEventRaised = null;
    //    seekRequestChannel.OnEventRaised = null;
    //}
    #endregion

    [FoldoutGroup("Static Effect")]
    [SerializeField] private bool displayEffect = true;
    [SerializeField] private GameObject staticEffect;
    [FoldoutGroup("Static Effect")]
    [SerializeField] private float displayTime;


    #region -- // Event Subscribing / Unsubscribing // --
    private void OnEnable()
    {
        scenarioProgressorChannel.OnEventRaised += SwapScenario;
        seekRequestChannel.OnEventRaised += SeekTo;

        if (GameManager.Instance != null)
            GameManager.Instance.GamePausedEvent += TogglePause;
    }

    private void OnDisable()
    {
        scenarioProgressorChannel.OnEventRaised -= SwapScenario;
        seekRequestChannel.OnEventRaised -= SeekTo;

        if (GameManager.Instance != null)
            GameManager.Instance.GamePausedEvent -= TogglePause;
    }

    private void OnDestroy()
    {
        // Unsubscribe the Scenario from any events it 
        // subscribed to during the game.
        currentScenario?.UnInit();
    }
    #endregion


    private void Start()
    {
        SwapScenario(currentScenario);
    }

    /// <summary>
    /// Plays the current scenario.
    /// </summary>
    public void PlayCurrentScenario()
    {
        if(!videoParent.isPlaying)
        {
            if (displayEffect)
            {
                //StartCoroutine(ActivateStatic());
            }

            videoParent.Play();
            StartCoroutine(TrackTime());
        }
    }

    private IEnumerator ActivateStatic()
    {
        staticEffect.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        staticEffect.SetActive(false);
    }

    /// <summary>
    /// Stops the current scenario.
    /// </summary>
    public void StopCurrentScenario()
    {
        videoParent.Stop();
        StopAllCoroutines();
    }

    /// <summary>
    /// Swaps the current scenario with the provided scenario.
    /// </summary>
    /// <param name="scenario">The scenario to swap to.</param>
    public void SwapScenario(FMVScenarioSO scenario)
    {
        print("Swapping scenarios");
        StopCurrentScenario();

        videoParent.loopPointReached -= currentScenario.OnVideoOver;
        currentScenario.UnInit();

        ClearPopups();
        currentScenario = scenario;
        currentScenario.Init();
        if (GameManager.Instance)
        {
            GameManager.Instance.currentScenario = currentScenario;
        }
        videoParent.loopPointReached += currentScenario.OnVideoOver;

        videoParent.clip = currentScenario.VideoClip;

        PlayCurrentScenario();
    }

    /// <summary>
    /// Clears any remaining popups from the previous scenario.
    /// </summary>
    private void ClearPopups()
    {
        int childCount = videoParent.transform.childCount;
        print("Clearing popups. Child count is " + childCount);
        for (int i = 0; i < childCount; ++i)
        {
            Destroy(videoParent.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Broadcasts the elapsed time in the video as an event.
    /// </summary>
    private IEnumerator TrackTime()
    {
        while(true)
        {
            timeElapsedChannel.RaiseEvent(videoParent.time);

            yield return null;
        }
    }

    /// <summary>
    /// Seeks to the given time in the video clip.
    /// </summary>
    /// <param name="time">The time to seek to.</param>
    private void SeekTo(double time)
    {
        Debug.Log("[FMVManager]: Looping call accepted. Seeking to time " + time + " seconds.");
        videoParent.Stop();
        videoParent.time = time;
        videoParent.Play();
    }

    /// <summary>
    /// Toggles pausing the video playback.
    /// </summary>
    /// <param name="paused">True if playback should be paused.</param>
    private void TogglePause(bool paused)
    {
        if (paused)
        {
            videoParent.Pause();
        }
        else
        {
            videoParent.Play();
        }
    }
}