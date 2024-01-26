/*****************************************************************************
// File Name :         MenuItems.cs
// Author :            Kyle Grenier
// Creation Date :     09/17/2021
//
// Brief Description : Script dedicated to creating custom editor buttons.
*****************************************************************************/
using UnityEngine;
using UnityEditor;
using UnityEngine.Video;

namespace FMVMaker.CustomEditor.MenuItems
{
    /// <summary>
    /// Script dedicated to creating custom editor buttons.
    /// </summary>
    public class MenuItems : MonoBehaviour
    {
        [MenuItem("Tools/FMV Maker/Create FMV Button")]
        // Instantiates a button with pre-attached components to interact with the FMV system.
        private static void CreateFMVButton()
        {
            const string BTN_NAME = "FMVButton";
            GameObject buttonPrefab = Resources.Load<GameObject>(BTN_NAME);
            if (buttonPrefab == null)
            {
                Debug.LogWarning("[MenuItems]: Could not load FMV Button prefab. Check 'Resources' folder for GameObject named '" + BTN_NAME + "'.");
                return;
            }

            GameObject videoParent = GameObject.FindGameObjectWithTag("VideoParent");
            if (videoParent == null)
            {
                Debug.LogWarning("[MenuItems]: Could not find GameObject tagged 'VideoParent'. Aborting...");
                return;
            }

            VideoPlayer videoPlayer = videoParent.GetComponent<VideoPlayer>();
            if (videoPlayer == null)
            {
                Debug.LogWarning("[MenuItems]: GameObject tagged 'VideoParent' does not have the required VideoPlayer component. Aborting...");
                return;
            }

            FMVTimedObjectPopup popup = Instantiate(buttonPrefab, videoParent.transform).GetComponent<FMVTimedObjectPopup>();
            float popupTime;

            // If the game is currently running, we can get the current time
            // of the video player.
            if (Application.isPlaying)
            {
                print("[MenuItems]: Application is playing, so getting time from VideoPlayer...");
                popupTime = (float)videoPlayer.time;
            }
            // If the game is stopped, get the time elapsed from the PlayerPrefs.
            else
            {
                print("[MenuItems]: Application is stopped, so getting time from PlayerPrefs...");
                print("Time caught is " + PlayerPrefs.GetFloat("Time Elapsed"));
                popupTime = PlayerPrefs.GetFloat("Time Elapsed");
            }

            popup.PopupTime = popupTime;
        }

        [MenuItem("Tools/FMV Maker/Add FMV Component/Timed Popup")]
        // Adds the FMVTimedPopup script to the selected GameObject.
        private static void AddTimedPopupComponent()
        {
            GameObject selection = Selection.activeGameObject;

            selection.AddComponent<FMVTimedObjectPopup>();
        }

        [MenuItem("Tools/FMV Maker/Add FMV Component/Scenario Progressor")]
        // Adds the FMVScenarioProgressor script to the selected GameObject.
        private static void AddScenarioProgressor()
        {
            GameObject selection = Selection.activeGameObject;

            selection.AddComponent<FMVScenarioProgressor>();
        }
    }
}