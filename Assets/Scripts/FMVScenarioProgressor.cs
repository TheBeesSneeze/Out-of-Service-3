/*****************************************************************************
// File Name :         FMVScenarioProgressor.cs
// Author :            Kyle Grenier
// Creation Date :     09/16/2021
//
// Brief Description : Progresses to another scenario when called upon.
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Video;
using System.Collections;

[DisallowMultipleComponent]
public class FMVScenarioProgressor : MonoBehaviour
{
    [Tooltip("The channel to raise an event to to signal a change in scenarios.")]
    [HideIf("IsSet")]
    [SerializeField] private FMVScenarioChannelSO scenarioProgressorChannel;

    [Tooltip("The scenario this progressor leads into.")]
    [SerializeField] private FMVScenarioSO nextScenario;

    [Tooltip("Button colors")]
    [SerializeField] private Color baseColor;
    [SerializeField] private Color selectColor;

    private int choiceMade = 0;

    private bool isVideoOver;
    [SerializeField] private VideoPlayer vp;

    /// <summary>
    /// Requests to progress to the next scenario provided.
    /// </summary>
    public void ProgressScenario()
    {
        GameManager.Instance.subtitleBox.text = "";
        scenarioProgressorChannel.RaiseEvent(nextScenario);
    }

    private bool IsSet()
    {
        return scenarioProgressorChannel != null;
    }

    private void OnEnable()
    {
        vp = GameObject.FindGameObjectWithTag("VideoParent").GetComponent<VideoPlayer>();

        vp.loopPointReached += EndReached;

        isVideoOver = false;

        Debug.Log("Video over bool: " + isVideoOver);

        //StartCoroutine(VideoEnd());
    }

    private void Update()
    {
        QueueScenario();

        if(choiceMade == 1 && gameObject.tag == "Choice 1")
        {
            gameObject.GetComponent<Image>().color = selectColor;
            if (isVideoOver == true)
            {
                ProgressScenario();
            }
        }
        else if(choiceMade == 2 && gameObject.tag == "Choice 2")
        {
            gameObject.GetComponent<Image>().color = selectColor;
            if (isVideoOver == true)
            {
                ProgressScenario();
            }
        }
        else if(choiceMade == 3 && gameObject.tag == "Choice 3")
        {
            gameObject.GetComponent<Image>().color = selectColor;
            if (isVideoOver == true)
            {
                ProgressScenario();
            }
        }
        else
        {
            gameObject.GetComponent<Image>().color = baseColor;
        }
    }

    private void QueueScenario()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            choiceMade = 1;
            //Debug.Log("Choice: " + choiceMade + "\nVideo playing bool: " + vp.isPlaying);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            choiceMade = 2;
            //Debug.Log("Choice: " + choiceMade + "\nVideo playing bool: " + vp.isPlaying);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            choiceMade = 3;
            //Debug.Log("Choice: " + choiceMade + "\nVideo playing bool: " + vp.isPlaying);
        }
    }

    void EndReached(VideoPlayer _vp)
    {
        isVideoOver = true;

        Debug.Log("Video over bool: " + isVideoOver);
    }
}