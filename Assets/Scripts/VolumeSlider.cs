/*****************************************************************************
// File Name :         VolumeSlider.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : ADD BRIEF DESCRIPTION OF THE FILE HERE
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    private Slider slider;

    [Tooltip("The AudioMixer to control.")]
    [SerializeField] private AudioMixer mixer;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        mixer.GetFloat("MainVolume", out float val);
        slider.value = val;

        slider.onValueChanged.AddListener(UpdateMixerVolume);
    }

    /// <summary>
    /// Sets the master volume to the value set
    /// on the slider.
    /// </summary>
    /// <param name="val">The value to set the master volume to.</param>
    private void UpdateMixerVolume(float val)
    {
        mixer.SetFloat("MainVolume", val);
    }
}
