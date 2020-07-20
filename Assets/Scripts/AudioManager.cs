using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    public float volume = 0;

    public float minVolume = 0;
    
    public float maxVolume = 1;

    public float volumeSpeed = 0.1f;

    public float targetVolume = 0;

    public bool playing = false;
    
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        volume += Mathf.Clamp(targetVolume - volume, -volumeSpeed, volumeSpeed);
        volume = Mathf.Clamp(volume, minVolume, maxVolume);
        gameObject.GetComponent<AudioSource>().volume = volume;
    }

    public void start() {
        if (playing) return;
        volume = targetVolume;
        gameObject.GetComponent<AudioSource>().Play();
        playing = true;
    }

}
