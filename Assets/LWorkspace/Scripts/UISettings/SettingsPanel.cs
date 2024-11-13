using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private AudioMixer audioMixer;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(GameConstants.BGM_SAVE_KEY))
        {
            float bgmVolume = PlayerPrefs.GetFloat(GameConstants.BGM_SAVE_KEY);
            musicSlider.value = bgmVolume;
            OnMusicSliderValueChanged(bgmVolume);
        }

        if (PlayerPrefs.HasKey(GameConstants.SFX_SAVE_KEY))
        {
            float sfxVolume = PlayerPrefs.GetFloat(GameConstants.SFX_SAVE_KEY);
            sfxSlider.value= sfxVolume;
            OnSFXSliderValueChanged(sfxVolume);
        }
    }


    public void OnMusicSliderValueChanged(float value)
    {
        audioMixer.SetFloat(GameConstants.BGM_SAVE_KEY, value);

        PlayerPrefs.SetFloat(GameConstants.BGM_SAVE_KEY, value);
        PlayerPrefs.Save();
    }

    public void OnSFXSliderValueChanged(float value)
    {
        audioMixer.SetFloat(GameConstants.SFX_SAVE_KEY, value);
        PlayerPrefs.SetFloat(GameConstants.SFX_SAVE_KEY, value);
        PlayerPrefs.Save();
    }

    public void OnBackButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
