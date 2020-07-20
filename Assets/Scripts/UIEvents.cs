using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIEvents : MonoBehaviour {

    public GameObject levelWindow;
    public GameObject settingsWindow;
    public GameObject OKButton;
    public GameObject CancelButton;
    public GameObject CloseButton;
    public GameObject UsernameBox;
    public GameObject creditsWindow;
    public GameObject statsWindow;
    public GameObject statsText;
    public List<GameObject> levels;
    public List<string> levelRedirects;

    Save mod;

    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        HideLevels();
        HideSettings(false);
        AudioManager.Instance.targetVolume = 0.05f;
        AudioManager.Instance.start();
        for (int i = 0; i < levels.Count; i++) {
            int index = i;
            levels[index].GetComponent<Button>().onClick.AddListener(() => LoadLevel(index));
        }
        UsernameBox.GetComponent<InputField>().onValueChanged.AddListener((text) => UpdateMod(text));
        OKButton.GetComponent<Button>().onClick.AddListener(() => HideSettings(true));
        CancelButton.GetComponent<Button>().onClick.AddListener(() => HideSettings(false));
        CloseButton.GetComponent<Button>().onClick.AddListener(() => HideSettings(false));
        if (SaveManager.Instance.save.firstPlay) {
            SaveManager.Instance.save.name = "";
            CancelButton.GetComponent<Button>().interactable = false;
            ShowSettings();
        }
        if (StatManager.Instance.statsDisplay) {
            ShowStats(StatManager.Instance.stats);
            StatManager.Instance.statsDisplay = false;
        }
    }

    void Update() {
        
    }

    void Quit() {
        Application.Quit();
    }

    void ShowLevels() {
        if (settingsWindow.activeSelf || creditsWindow.activeSelf || statsWindow.activeSelf) return;
        int unlocked = SaveManager.Instance.save.unlocked;
        for (int i = 0; i < levels.Count; i++) {
            levels[i].SetActive(i < unlocked);
        }
        levelWindow.SetActive(true);
    }

    void HideLevels() {
        levelWindow.SetActive(false);
    }

    void ShowSettings() {
        if (levelWindow.activeSelf || creditsWindow.activeSelf || statsWindow.activeSelf) return;
        mod = SaveManager.Instance.save;
        UsernameBox.GetComponent<InputField>().text = mod.name;
        settingsWindow.SetActive(true);
    }

    void UpdateMod(string update) {
        mod.name = update;
    }

    void HideSettings(bool save) {
        settingsWindow.SetActive(false);
        CancelButton.GetComponent<Button>().interactable = true;
        if (save) {
            SaveManager.Instance.save = mod;
            SaveManager.Instance.saveSave();
            SaveManager.Instance.loadSave();
        }
    }

    void Credits() {
        if (settingsWindow.activeSelf || levelWindow.activeSelf || statsWindow.activeSelf) return;
        creditsWindow.SetActive(true);
    }

    void HideCredits() {
        creditsWindow.SetActive(false);
    }

    void ShowStats(Stats stats) {
        if (settingsWindow.activeSelf || levelWindow.activeSelf || creditsWindow.activeSelf) return;
        statsText.GetComponent<Text>().text = stats.format();
        statsWindow.SetActive(true);
    }

    void HideStats() {
        statsWindow.SetActive(false);
    }

    public void LoadLevel(int button) {
        SceneManager.LoadScene(levelRedirects[button]);
    }
}
