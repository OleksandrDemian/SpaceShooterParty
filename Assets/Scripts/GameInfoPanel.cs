using UnityEngine.UI;
using UnityEngine;

public class GameInfoPanel : MonoBehaviour
{
    private GameInfo info;
    private Text txtGameTime;
    private Text txtCameraSizeSlider;

    private void Start()
    {
        int savedTime = PlayerPrefs.GetInt("Time", 2);
        int savedSize = PlayerPrefs.GetInt("Size", 12);
        bool bAsteroids = PlayerPrefs.GetString("Asteroids", "1") == "1" ? true : false;
        bool bShields = PlayerPrefs.GetString("Shields", "1") == "1" ? true : false;
        bool bbonuses = PlayerPrefs.GetString("Bonuses", "1") == "1" ? true : false;
        bool bAudio = PlayerPrefs.GetString("Audio", "1") == "1" ? true : false;

        info = new GameInfo(savedTime, savedSize, bAsteroids, bShields, bbonuses);

        Slider timeSlider = transform.FindChild("TimeSlider").GetComponent<Slider>();
        txtGameTime = transform.FindChild("GameTime").GetComponent<Text>();

        Slider cameraSizeSlider = transform.FindChild("MapSizeSlider").GetComponent<Slider>();
        txtCameraSizeSlider = transform.FindChild("MapSize").GetComponent<Text>();

        Toggle asteroids = transform.FindChild("Asteroids").GetComponent<Toggle>();
        Toggle shields = transform.FindChild("Shields").GetComponent<Toggle>();
        Toggle bonus = transform.FindChild("Bonuses").GetComponent<Toggle>();
        Toggle audio = transform.FindChild("Audio").GetComponent<Toggle>();

        timeSlider.onValueChanged.AddListener(OnSliderTimeValueChange);
        cameraSizeSlider.onValueChanged.AddListener(OnSliderMapSizeValueChange);
        asteroids.onValueChanged.AddListener(OnAsteroidBoolValueChange);
        shields.onValueChanged.AddListener(OnShieldsBoolValueChange);
        bonus.onValueChanged.AddListener(OnBonusBoolValueChange);
        audio.onValueChanged.AddListener(OnAudioBoolValueChange);

        timeSlider.value = savedTime;
        cameraSizeSlider.value = savedSize;
        asteroids.isOn = bAsteroids;
        shields.isOn = bShields;
        bonus.isOn = bbonuses;
        audio.isOn = bAudio;
    }

    private void OnSliderMapSizeValueChange(float value)
    {
        txtCameraSizeSlider.text = "Map size: " + value;
        info.MapSize = (int)value;
        PlayerPrefs.SetInt("Size", (int)value);
        Debug.Log("Size: " + value + " <-> " + info.MapSize + " <-> " + PlayerPrefs.GetInt("Size"));
    }

    private void OnSliderTimeValueChange(float value)
    {
        PlayerPrefs.SetInt("Time", (int)value);
        value *= 30;
        txtGameTime.text = "Game time: " + value + " seconds";
        info.MatchTime = (int)value;
    }

    private void OnAsteroidBoolValueChange(bool value)
    {
        info.AsteroidsEnabled = value;
        PlayerPrefs.SetString("Asteroids", value ? "1" : "0");
    }

    private void OnShieldsBoolValueChange(bool value)
    {
        info.ShieldsEnabled = value;
        PlayerPrefs.SetString("Shields", value ? "1" : "0");
        Debug.Log("Shield: " + value + " -> " + info.ShieldsEnabled);
    }

    private void OnBonusBoolValueChange(bool value)
    {
        info.BonusesEnbled = value;
        PlayerPrefs.SetString("Bonuses", value ? "1" : "0");
    }

    private void OnAudioBoolValueChange(bool value)
    {
        info.AudioEnabled = value;
        PlayerPrefs.SetString("Audio", value ? "1" : "0");
    }

}
