using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    // Audio
    public Slider volumeSlider;
    public Toggle musicPlayerToggle;
    // Controls
    public TMP_Dropdown turnControlsDropdown;
    public TMP_Dropdown grabControlsDropdown;

    void OnEnable(){
        // Audio
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        musicPlayerToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("musicPlayer"));
        // Controls
        turnControlsDropdown.value = PlayerPrefs.GetInt("turnMode");
        grabControlsDropdown.value = PlayerPrefs.GetInt("grabMode");
    }
    void Update()
    {
        // Audio
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        PlayerPrefs.SetInt("musicPlayer", System.Convert.ToInt16(musicPlayerToggle.isOn));
        // Controls
        PlayerPrefs.SetInt("turnMode", turnControlsDropdown.value);
        PlayerPrefs.SetInt("grabMode", grabControlsDropdown.value);
    }
}
