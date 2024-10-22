using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject player, UI, Menu, SettingsMenu, PP;
    private PostProcessVolume ppVolume;
    private ColorGrading colorGrading;
    private PlayerController script;

    public Slider volume;
    public Slider brightness;

    // Start is called before the first frame update
    void Start()
    {
        script = player.GetComponent<PlayerController>();
        ppVolume = PP.GetComponent<PostProcessVolume>();
        ppVolume.profile.TryGetSettings(out colorGrading);
        volume.value = 0.75f;
        brightness.value = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = volume.value;
        colorGrading.postExposure.value = brightness.value * 2;
    }

    public void Resume()
    {
        script.Paused = !script.Paused;
        Time.timeScale = 1;
        UI.SetActive(!script.Paused);
        gameObject.SetActive(script.Paused);
    }

    public void Settings()
    {
        Menu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void Leave()
    {
        Application.Quit();
    }

    public void ExitSettings()
    {
        Menu.SetActive(true);
        SettingsMenu.SetActive(false);
    }
}
