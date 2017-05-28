using UnityEngine.UI;
using UnityEngine;

public class GameInfoPanel : MonoBehaviour
{
    private GameInfo info;
    private Text txtGameTime;
    private Text txtCameraSizeSlider;

    private void Start()
    {
        info = new GameInfo();
        Slider timeSlider = transform.FindChild("TimeSlider").GetComponent<Slider>();
        txtGameTime = transform.FindChild("GameTime").GetComponent<Text>();

        Slider cameraSizeSlider = transform.FindChild("MapSizeSlider").GetComponent<Slider>();
        txtCameraSizeSlider = transform.FindChild("MapSize").GetComponent<Text>();

        Toggle asteroids = transform.FindChild("Asteroids").GetComponent<Toggle>();
        Toggle shields = transform.FindChild("Shields").GetComponent<Toggle>();
        Toggle bonus = transform.FindChild("Bonuses").GetComponent<Toggle>();

        timeSlider.onValueChanged.AddListener(OnSliderTimeValueChange);
        cameraSizeSlider.onValueChanged.AddListener(OnSliderMapSizeValueChange);

        asteroids.onValueChanged.AddListener(OnAsteroidBoolValueChange);
        shields.onValueChanged.AddListener(OnShieldsBoolValueChange);
        bonus.onValueChanged.AddListener(OnBonusBoolValueChange);
    }

    private void OnSliderMapSizeValueChange(float value)
    {
        txtCameraSizeSlider.text = "Map size: " + value;
        info.MapSize = (int)value;
    }

    private void OnSliderTimeValueChange(float value)
    {
        value *= 30;
        txtGameTime.text = "Game time: " + value + " seconds";
        info.MatchTime = (int)value;
    }

    private void OnAsteroidBoolValueChange(bool value)
    {
        info.AsteroidsEnabled = value;
    }

    private void OnShieldsBoolValueChange(bool value)
    {
        info.ShieldsEnabled = !value;
    }

    private void OnBonusBoolValueChange(bool value)
    {
        info.BonusesEnbled = value;
    }

}
