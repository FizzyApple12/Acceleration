using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class StatManager : MonoBehaviour {

    public static StatManager Instance { get; private set; }
    public GameObject player;
    public bool statsDisplay = false;
    public Stats stats;
    bool recording = false;

    //RECORDED
    
    Stopwatch time;
    Stopwatch air;
    Stopwatch ground;
    float maxSpeed = 0;
    
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
        if (recording) {
            float speed = player.GetComponent<Rigidbody>().velocity.magnitude;
            if (speed > maxSpeed) maxSpeed = speed;

            if ((player.GetComponent<Player>().touchingFloor || player.GetComponent<Player>().touchingWall) && !ground.IsRunning) ground.Start();
            if (!player.GetComponent<Player>().touchingFloor && !player.GetComponent<Player>().touchingWall && ground.IsRunning) ground.Stop();

            if (!player.GetComponent<Player>().touchingFloor && !player.GetComponent<Player>().touchingWall && !air.IsRunning) air.Start();
            if ((player.GetComponent<Player>().touchingFloor || player.GetComponent<Player>().touchingWall) && air.IsRunning) air.Stop();
        }
    }

    public void startRecording(GameObject go) {
        player = go;
        time = new Stopwatch();
        air = new Stopwatch();
        ground = new Stopwatch();
        time.Start();
        recording = true;
    }

    public void stopRecording(bool discard) {
        time.Stop();
        if (air.IsRunning) air.Stop();
        if (ground.IsRunning) ground.Stop();
        recording = false;
        if (discard) return;
        statsDisplay = true;
        stats = new Stats();
        stats.time = time.Elapsed;
        stats.air = air.Elapsed;
        stats.ground = ground.Elapsed;
        stats.maxSpeed = maxSpeed;
    }
}
