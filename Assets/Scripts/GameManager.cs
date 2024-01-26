/*****************************************************************************
// File Name :         GameManager.cs
// Author :            Kyle Grenier
// Creation Date :     09/29/2021
//
// Brief Description : Handle's managing the game's state.
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Audio;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Handles managing the game's state.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// True if the game is paused.
    /// </summary>
    private bool paused;

    /// <summary>
    /// Event called when the game's pause state is toggled.
    /// </summary>
    public UnityAction<bool> GamePausedEvent;

    [Tooltip("The pause menu UI.")]
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private UnityEngine.Video.VideoPlayer mainPlayer;
    [SerializeField] private UnityEngine.UI.Slider volSlider;
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private DoubleChannelSO seekRequestChannel;

    [HideInInspector]public FMVScenarioSO currentScenario;
    public TextMeshProUGUI subtitleBox;
    public TextMeshProUGUI subtitleBackdropBox;
    private bool curSubComplete;
    private int subIndex;

    /// <summary>
    /// Member variable initialization.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        paused = false;
        pauseMenu.SetActive(false);
        curSubComplete = false;
        subIndex = 0;

        Cursor.visible = false;

        volSlider.onValueChanged.AddListener(UpdateVideoVolume);
    }

    private void Start()
    {
        mainMixer.GetFloat("MainVolume", out float val);
        UpdateVideoVolume(val);
    }

    private void UpdateVideoVolume(float val)
    {
        const float MAX_VAL = 100;

        float acceptedVal = val + 80;

        mainPlayer.SetDirectAudioVolume(0, acceptedVal / MAX_VAL);
    }

    private void Update()
    {
        // Enable the pause menu if the escape key is pressed.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            skipScene();
        }
        //if (currentScenario.doesLoop() && currentScenario.timeElapsed >= currentScenario.getLoopBeginTime())
        //{
        //    subtitleBox.transform.position = new Vector3(457, 110, 0);
        //}
        //else
        //{
        //    subtitleBox.transform.position = new Vector3(457, 25, 0);
        //}

        subtitleUpdate();
    }

    public void subtitleUpdate()
    {
        if (currentScenario != null && currentScenario.speechText != null && currentScenario.beginTime != null && currentScenario.endTime != null)
        {
            if (subIndex < currentScenario.speechText.Count)
            {
                if (Mathf.Abs((float)(currentScenario.timeElapsed - currentScenario.beginTime[subIndex])) <= .1)
                {
                    subtitleBox.text = currentScenario.speechText[subIndex];
                    subtitleBackdropBox.text = "<mark=#000000>" + currentScenario.speechText[subIndex];
                }

                if (Mathf.Abs((float)(currentScenario.timeElapsed - currentScenario.endTime[subIndex])) <= .1)
                {
                    subtitleBox.text = "";
                    subtitleBackdropBox.text = "";
                    curSubComplete = true;
                }

                if (curSubComplete)
                {
                    subIndex++;
                    curSubComplete = false;
                }
            }
            else
            {
                subIndex = 0;
            }
        }
    }

    public void TogglePause()
    {
        paused = !paused;

        if (paused == true)
            Cursor.visible = true;
        else if (paused == false)
            Cursor.visible = false;

        pauseMenu.SetActive(paused);
        GamePausedEvent?.Invoke(paused);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void skipScene()
    {
        subIndex = 0;
        subtitleBox.text = "";
        subtitleBackdropBox.text = "";
        seekRequestChannel.RaiseEvent(mainPlayer.clip.length - 0.1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}