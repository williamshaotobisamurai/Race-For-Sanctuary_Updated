using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        // Initialize the slider value, e.g., from saved settings
        volumeSlider.value = AudioListener.volume;
    }

    public void OnSliderValueChanged(float value)
    {
        AudioListener.volume = 1.0f;
        Debug.Log("Start");
    }
      
}
