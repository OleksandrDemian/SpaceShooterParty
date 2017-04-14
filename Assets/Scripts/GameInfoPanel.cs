using UnityEngine.UI;
using UnityEngine;

public class GameInfoPanel : MonoBehaviour {

    private Slider timeSlider;
    private Text txtGameTime;
    private GameInfo info;

    private void Start()
    {
        info = new GameInfo();
        timeSlider = transform.FindChild("TimeSlider").GetComponent<Slider>();
        txtGameTime = transform.FindChild("GameTime").GetComponent<Text>();

        Toggle asteroids = transform.FindChild("Asteroids").GetComponent<Toggle>();
        Toggle shields = transform.FindChild("Shields").GetComponent<Toggle>();

        timeSlider.onValueChanged.AddListener(OnSliderValueChange);
        asteroids.onValueChanged.AddListener(OnAsteroidBoolValueChange);
        shields.onValueChanged.AddListener(OnShieldsBoolValueChange);
    }

    private void OnSliderValueChange(float value)
    {
        txtGameTime.text = "Game time: " + value + " seconds";
        info.gameTime = (int)value;
    }

    private void OnAsteroidBoolValueChange(bool value)
    {
        info.enableAsteroids = value;
    }

    private void OnShieldsBoolValueChange(bool value)
    {
        info.shieldsDisabled = value;
    }

}
