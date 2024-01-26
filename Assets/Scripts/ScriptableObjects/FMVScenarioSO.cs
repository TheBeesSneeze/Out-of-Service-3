/*****************************************************************************
// File Name :         FMVScenarioSO.cs
// Author :            Kyle Grenier
// Creation Date :     09/16/2021
//
// Brief Description : A SO that represents a "scenario" in the FMV movie.
                       I'm defining a scenario as a scene in the movie / a point in the game 
                       that the player has to make choices get to.

                       A scenario may have options the player has to make: do they choose path A or path B?
                       That choice will lead to yet another scenario. This means that scenarios are connected in a sort of
                       tree structure.
*****************************************************************************/
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Video;

[HideMonoScript]
[CreateAssetMenu(menuName = "FMV Maker/Scenarios/Scenario", fileName = "New Scenario")]
public class FMVScenarioSO : ScriptableObject
{
    [Tooltip("The video clip to play during this scenario.")]
    [SerializeField] private VideoClip videoClip;
    /// <summary>
    /// The VideoClip to play during this scenario.
    /// </summary>
    public VideoClip VideoClip
    {
        get
        {
            return videoClip;
        }
    }

    #region -- // Channels // --
    [Tooltip("The channel to receive video time elapsed calls from.")]
    [HideIf("TimeElapsedChannelSet")]
    [SerializeField] private DoubleChannelSO timeElapsedChannel;
    /// <summary>
    /// Returns true if the timeElapsedChannel is not null.
    /// </summary>
    /// <returns>True if the timeElapsedChannel is not null.</returns>
    private bool TimeElapsedChannelSet()
    {
        return timeElapsedChannel != null;
    }

    [Tooltip("The channel to broadcast scenario progression events to.")]
    [HideIf("IsProgressorChannelSet")]
    [SerializeField] private FMVScenarioChannelSO scenarioProgressorChannel;
    /// <summary>
    /// Returns true if the scenarioProgressorChannel is not null.
    /// </summary>
    /// <returns>True if scenarioProgressorChannel is not null.</returns>
    private bool IsProgressorChannelSet()
    {
        return scenarioProgressorChannel != null;
    }
    #endregion

    #region -- // Choice Selection // --
    [Tooltip("True if the player will be required to make a choice.\n" +
             "False if scenario will automatically play into the next.")]
    [SerializeField] private bool choicesToBeMade;
    /// <summary>
    /// True if the player will be required to make a choice.
    /// False if scenario will automatically play into the next
    /// </summary>
    public bool ChoicesToBeMade
    {
        get
        {
            return choicesToBeMade;
        }
    }

    // Choices to be made
    [Tooltip("An array of popups that are instantiated in the scenario.")]
    [ShowIf("choicesToBeMade")]
    [AssetsOnly]
    [SerializeField] private FMVTimedObjectPopup[] popups;
    /// <summary>
    /// A clone of the popups array so we don't modify the 
    /// actual array of the ScriptableObject.
    /// </summary>
    private List<FMVTimedObjectPopup> popupsClone;
    /// <summary>
    /// How many popups still need to be instantiated.
    /// </summary>
    private int popupsLength;

    // No choices to be made
    [Tooltip("The progressor Scenario ScriptableObject to progress into " +
        "if no choices are to be made.")]
    [HideIf("choicesToBeMade")]
    [Required("Video will abruptly pause at the end of the current video is there is no next scenario and looping is not enabled.", InfoMessageType.Info)]
    [SerializeField] private FMVScenarioSO nextScenario;
    #endregion

    #region -- // Looping // --
    [Tooltip("True if this scenario should loop.")]
    [SerializeField] private bool enableLooping;
    public bool doesLoop()
    {
        return enableLooping;
    }

    [Tooltip("The channel to broadcast time seek requests to.")]
    [ShowIfGroup("enableLooping")]
    [BoxGroup("enableLooping/Looping Data")]
    [HideIf("IsTimeSeekChannelSet")]
    [SerializeField] private DoubleChannelSO seekRequestChannel;
    /// <summary>
    /// Returns true if the timeSeekChannel is not null.
    /// </summary>
    /// <returns>True if the timeSeekChannel is not null.</returns>
    private bool IsTimeSeekChannelSet()
    {
        return seekRequestChannel != null;
    }

    [BoxGroup("enableLooping/Looping Data")]
    [Tooltip("The beginning of the loop sequence (in time elapsed).")]
    [InlineButton("SetDefaultLoopBeginningTime", "Current time of clip")]
    [SerializeField][Min(0)] private double loopBeginningTime;
    public double getLoopBeginTime()
    {
        return loopBeginningTime;
    }

    [BoxGroup("enableLooping/Looping Data")]
    [Tooltip("The time the loop will begin (in time elapsed).")]
    [InlineButton("SetDefaultLoopEnterTime", "End of clip")]
    [SerializeField][Min(0)] private double loopEnterTime;

    /// <summary>
    /// Sets the loopEnterTime to the length of the video clip.
    /// </summary>
    private void SetDefaultLoopEnterTime()
    {
        loopEnterTime = videoClip.length - 0.2;
    }
    /// <summary>
    /// Sets the loopBeginningTime to the time the last video played was stopped, or the current time of the 
    /// currently playing video.
    /// </summary>
    private void SetDefaultLoopBeginningTime()
    {
        if (Application.isPlaying)
        {
            loopBeginningTime = timeElapsed;
        }
        else if (PlayerPrefs.HasKey("Time Elapsed"))
        {
            loopBeginningTime = PlayerPrefs.GetFloat("Time Elapsed");
        }
        else
        {
            Debug.LogWarning("[FMVScenario]: Could not retrieve time elapsed. Has the scene been played at least once?");
        }
    }
    #endregion

    #region -- // Callback Channels // --
    [InfoBox("These Scenario Callback are optional. If assigned, they will be raised at the start and end of the Scenario respectively.")]
    [FoldoutGroup("Scenario Callbacks")]
    [Tooltip("The Scenario's (optional) on-start callback.")]
    [SerializeField] private ScenarioCallbackSO scenarioStartCallbackChannel;
    [FoldoutGroup("Scenario Callbacks")]
    [Tooltip("The Scenario's (optional) on-end callback.")]
    [SerializeField] private ScenarioCallbackSO scenarioEndCallbackChannel;
    #endregion

    #region -- // Subtitles // --
    public List<string> speechText;
    public List<double> beginTime;
    public List<double> endTime;
    #endregion

    /// <summary>
    /// The time elapsed in the current scenario.
    /// </summary>
    public double timeElapsed;

    /// <summary>
    /// Initializes the current scenario.
    /// </summary>
    public void Init()
    {
        UnInit();

        popupsLength = popups.Length;
        popupsClone = new List<FMVTimedObjectPopup>(popups);


        timeElapsedChannel.OnEventRaised += HandlePopups;

        timeElapsedChannel.OnEventRaised += TrackTime;

        if (enableLooping)
        {
            if (!ValidateLoopData())
            {
                Debug.LogWarning("[FMVScenario]: Scenario '" + name + "' has invalid loop data. Disabling looping. " +
                    "Please refer to the above warnings and/or the documentation for more details.");
                return;
            }

            Debug.Log("Looping enabled...");

            timeElapsedChannel.OnEventRaised += HandleLooping;
        }

        scenarioStartCallbackChannel?.PerformCallback(this);
    }

    /// <summary>
    /// Returns true if the provided loop data is valid.
    /// </summary>
    /// <returns>True if the provided loop data is valid.</returns>
    private bool ValidateLoopData()
    {
        if (loopBeginningTime < 0 || loopEnterTime > videoClip.length)
        {
            Debug.LogWarning("[FMVScenario]: Scenario '" + name + "' has a loop time outside of the video clip's length.");
            return false;
        }

        if (loopEnterTime <= 0)
        {
            Debug.Log("[FMVScenario]: Scenario '" + name + "' has a loop enter time of 0 or less. Setting loop enter time to the video's end.");
            SetDefaultLoopEnterTime();
        }

        if (loopBeginningTime >= loopEnterTime)
        {
            Debug.LogWarning("[FMVScenario]: Scenario '" + name + "' has a loop end time greater than the loop start time.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Unsubscribes from the time elapsed channel's event.
    /// </summary>
    public void UnInit()
    {
        timeElapsedChannel.OnEventRaised -= HandlePopups;
        timeElapsedChannel.OnEventRaised -= HandleLooping;
        timeElapsedChannel.OnEventRaised -= TrackTime;
        timeElapsed = 0;
    }

    /// <summary>
    /// Handles instantiating popups at the correct time.
    /// </summary>
    /// <param name="elapsedTime">The elapsed time in the current scenario.</param>
    private void HandlePopups(double elapsedTime)
    {
        // If there are no popups in the array, 
        // unsubscribe from the event to prevent function calls.
        if (popupsLength <= 0)
        {
            timeElapsedChannel.OnEventRaised -= HandlePopups;
            return;
        }

        // Loop through the array of popups and instantiate any
        // that hit their popup time.
        for (int i = 0; i < popupsClone.Count; ++i)
        {
            FMVTimedObjectPopup popup = popupsClone[i];
            if (popup == null)
            {
                continue;
            }

            if (elapsedTime >= popup.PopupTime)
            {
                InstantiatePopup(popup.gameObject);
                popupsClone[i] = null;
                popupsLength--;
            }
        }
    }

    /// <summary>
    /// Keeps track of the elapsed time of the playing scenario.
    /// </summary>
    /// <param name="time">The elapsed time of the playing scenario.</param>
    private void TrackTime(double time)
    {
        timeElapsed = time;
    }

    /// <summary>
    /// Handles looping the scenario at the correct times.
    /// </summary>
    /// <param name="elapsedTime">The elapsed time in the current scenario.</param>
    private void HandleLooping(double elapsedTime)
    {
        // If the elapsed time is greater than or equal to the loop's
        // end time (where the looping occurs), request to seek the time to the loop's
        // beginning.
        if (elapsedTime >= loopEnterTime)
        {
            Debug.Log("[Scenario]: Requesting to loop from time " + loopEnterTime + " seconds to " + loopBeginningTime + " seconds.");
            seekRequestChannel.RaiseEvent(loopBeginningTime);
        }
    }

    /// <summary>
    /// Invoked when the video is over.
    /// </summary>
    /// <param name="source">The VideoPlayer that ended.</param>
    public void OnVideoOver(VideoPlayer source)
    {
        scenarioEndCallbackChannel?.PerformCallback(this);

        // If there are no choices to be made and we want to progress immediately into
        // the next scenario, invoke the RequestScenarioProgression() method.
        if (!choicesToBeMade)
        {
            // If there are no choices to be made, make sure the
            // next scenario isn't null so we don't get an error.
            if (nextScenario != null)
            {
                scenarioProgressorChannel.RaiseEvent(nextScenario);
            }
        }
    }

    /// <summary>
    /// Instantiates a popup into the world with the game object tagged "VideoParent" as the parent. 
    /// </summary>
    /// <param name="popup">The popup to instantiate.</param>
    private void InstantiatePopup(GameObject popup)
    {
        Transform videoParent = GameObject.FindGameObjectWithTag("VideoParent").transform;
        Instantiate(popup, videoParent);
    }

    /// <summary>
    /// Adds a popup to the list of popups to be instantiated.
    /// 
    /// NOTE: The popup can only be added during the 
    /// start of the Scenario after initialization. 
    /// 
    /// Therefore, this method should be called through a Scenario start callback.
    /// </summary>
    public void AddPopup(FMVTimedObjectPopup popup)
    {
        if (popupsClone != null)
        {
            Debug.Log("Adding popup to clone");
            popupsClone.Add(popup);
            ++popupsLength;
        }
    }
}