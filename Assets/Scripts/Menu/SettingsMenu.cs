using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainAudio;
    public Resolution[] resolutions;
    public Dropdown dropDownResolutions;

    public void Awake()
    {
        resolutions = Screen.resolutions;
        dropDownResolutions.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        dropDownResolutions.AddOptions(options);
        dropDownResolutions.value = currentResolutionIndex;
        dropDownResolutions.RefreshShownValue();
        
    }

    public void SetVolume(float volume) {
        mainAudio.SetFloat("volume", volume);
    }

    public void SetQualityLevel(int indexQuality)
    {
        QualitySettings.SetQualityLevel(indexQuality);
    }

    public void SetResolution(int indexResolution) {
        Resolution resolution = resolutions[indexResolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void setFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
