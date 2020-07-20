using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stats {
    public TimeSpan time;
    public float maxSpeed;
    public TimeSpan ground;
    public TimeSpan air;

    public string format() {
        string built = "";

        built += "Level Completed!\n\n";
        built += String.Format("Total Time: {0:00}:{1:00}.{2:00}\n", time.Minutes, time.Seconds, time.Milliseconds / 10);
        built += String.Format("Max Speed: {0} m/s\n", maxSpeed);
        built += String.Format("Spent {0:00}:{1:00}.{2:00} on the ground\n", ground.Minutes, ground.Seconds, ground.Milliseconds / 10);
        built += String.Format("Spent {0:00}:{1:00}.{2:00} in the air\n", air.Minutes, air.Seconds, air.Milliseconds / 10);

        return built;
    }

}
