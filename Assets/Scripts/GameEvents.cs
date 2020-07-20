using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEvents : MonoBehaviour {

    public bool levelComplete = false;
    public bool levelFailed = false;
    public int levelNumber;

    void Start() {
        if (AudioManager.Instance != null) {
            if (levelNumber == 6 || levelNumber == 12) {
                AudioManager.Instance.targetVolume = AudioManager.Instance.maxVolume / 2;
            } else {
                AudioManager.Instance.targetVolume = AudioManager.Instance.maxVolume;
            }
        }
    }

    void Update() {
        if (levelComplete) {
            if (StatManager.Instance != null) StatManager.Instance.stopRecording(false);
            SceneManager.LoadScene("Main UI");
            if (SaveManager.Instance != null && SaveManager.Instance.save.unlocked < levelNumber + 1) {
                SaveManager.Instance.save.unlocked = levelNumber + 1;
                SaveManager.Instance.saveSave();
            }
        }
        if (levelFailed) {
            if (StatManager.Instance != null) StatManager.Instance.stopRecording(true);
            SceneManager.LoadScene("Death");
        }
    }

    public void FailLevel() {
        levelFailed = true;
    }

    public void CompleteLevel() {
        levelComplete = true;
    }
}
