using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour {
    void Start() {
        if (AudioManager.Instance != null) AudioManager.Instance.targetVolume = AudioManager.Instance.minVolume;
        Cursor.visible = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.Return)) SceneManager.LoadScene("Main UI");
    }
}
