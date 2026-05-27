using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Slider musicSlider;
    public Text volumeText;
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Music Volume", 1f);
        musicSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);

        volumeText.text = Mathf.RoundToInt(musicSlider.value * 100) + "%";
    }
}