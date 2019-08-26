using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas: MonoBehaviour
{
    public GameObject Panel;
    public GameObject PanelOptions;
    public Slider sliderDirection;
    public Slider sliderSpeed;
    public Text sliderDirectionLabelValue;
    public Text sliderSpeedLabelValue;
    public GameObject windDirection;
    private GameManager gameManagerScript;
    public Dropdown dropdownMode;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void MenuOn()
    {
        Panel.SetActive(false);
        PanelOptions.SetActive(true);
    }
    public void MenuOff()
    {
        Panel.SetActive(true);
        PanelOptions.SetActive(false);
    }

    public void ModeChange() {

        switch (dropdownMode.GetComponentInChildren<Text>().text) {
            case "Add":
                gameManagerScript.clickMode = GameManager.clickModeClass.ADD;
                break;
            case "Remove":
                gameManagerScript.clickMode = GameManager.clickModeClass.REMOVE;
                break;
            case "Fire":
                gameManagerScript.clickMode = GameManager.clickModeClass.FIRE;
                break;
        }
        
    }

    public void changeWindSpeedSlider()
    {
        gameManagerScript.changeWindSpeed((int)sliderSpeed.value);
        sliderSpeedLabelValue.GetComponent<Text>().text = sliderSpeed.value.ToString();
    }

    public void changeWindDirectionSlider()
    {
        windDirection.gameObject.GetComponent<RectTransform>().SetPositionAndRotation(windDirection.gameObject.GetComponent<RectTransform>().position, Quaternion.Euler(0, 180, sliderDirection.value));
        sliderDirectionLabelValue.GetComponent<Text>().text = sliderDirection.value.ToString();

    }

    public void fireButton() {
        gameManagerScript.randomFire();
    }

    public void QuitButton()
    {
        gameManagerScript.QuitGame();
    }
}
