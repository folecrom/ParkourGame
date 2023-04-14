using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;// Référence à un Slider utilisé pour régler le volume
    [SerializeField] private TMP_Dropdown resolutionDropDown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private AudioMixer audioMixer;  // Référence à un AudioMixer utilisé pour gérer le mixage audio

    private Resolution[] resolutions;
    private int currentResolutionID;

    private void Awake()
    {
        //Init Resolutions
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();

        List<string> _resolutionLabels = new List<string>();
        for (var i = 0; i < resolutions.Length; i++)
        {
            _resolutionLabels.Add(resolutions[i].width + "x" + resolutions[i].height);
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) currentResolutionID = i;
        }

        resolutionDropDown.AddOptions(_resolutionLabels);
        
        //Init les valeurs
        resolutionDropDown.value = currentResolutionID;
        fullscreenToggle.isOn = Screen.fullScreen;
        audioMixer.GetFloat("Master", out float _volume);
        volumeSlider.value = Mathf.InverseLerp(-60f, 5f, _volume);

        //Link les events
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
        resolutionDropDown.onValueChanged.AddListener(UpdateResolution);
        fullscreenToggle.onValueChanged.AddListener(ToggleFullscren);
    }

    private void UpdateVolume(float _value)
    {
        audioMixer.SetFloat("Master", Mathf.Lerp(-40, 0.0f, _value));
        print("Audio Mixer : " + _value);
    }

    private void UpdateResolution(int _value)
    {
        currentResolutionID = _value;
        Screen.SetResolution(resolutions[currentResolutionID].width, resolutions[currentResolutionID].height, Screen.fullScreen);
        print("Resolution : " + resolutions[currentResolutionID]);
    }

    private void ToggleFullscren(bool _value)
    {
        Screen.fullScreen = _value;
        print("Fullscreen : " + Screen.fullScreen);
    }
}
