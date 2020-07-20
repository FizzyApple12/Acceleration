using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {
    public int unlocked = 1; // level player has unlocked to
    public bool firstPlay = true; // is this the player's first launch
    public string name = "user"; // name of player

}
