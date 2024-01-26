/*****************************************************************************
// File Name :         Timer.cs
// Author :            Toby Schamberger
// Creation Date :     4/7/2023
//
// Brief Description : Makes timer appear at same time as buttons. Counts down 
// until video ends.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Video;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using Image = UnityEngine.UI.Image;

public class Timer : MonoBehaviour
{
    public Slider TimerSlider;
    public VideoPlayer Video;

    private Image buttonImage;
    
    public double TimeElapsed;
    public double TimeOfButtonAppear;
    public double VideoLength;

    void Start()
    {
        TimerSlider.gameObject.SetActive(false);
        StartCoroutine(UpdateSlider());
    }

    public IEnumerator UpdateSlider()
    {
        while(true)
        {
            TimerSlider.gameObject.SetActive(false);

            //Searches for buttons on screen
            while (buttonImage == null)
            {
                buttonImage = GetButtonImage();
                yield return new WaitForSeconds(0.1f);
            }
            
            //Activates slider
            TimerSlider.gameObject.SetActive(true);

            VideoLength = Video.length;
            TimeOfButtonAppear = Video.clockTime;
            TimeElapsed = Video.clockTime;

            while (TimeElapsed >= TimeOfButtonAppear)
            {
                TimeElapsed = Video.clockTime;
                TimerSlider.value = (float)( (TimeElapsed - TimeOfButtonAppear) / (VideoLength - TimeOfButtonAppear) );

                yield return null;
            }

            buttonImage = null;

        }
    }

    public Image GetButtonImage()
    {
        GameObject button;

        for(int i =1; i<= 3; i++ )
        {
            button = GameObject.FindGameObjectWithTag("Choice " + i);
            if (button != null)
            {
                return button.GetComponent<Image>();
            }
        }

        return null;
    }
}
