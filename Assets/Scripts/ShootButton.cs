using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootButton : MonoBehaviour {
    public bool active = false;
    void Start() {
        
    }
 
    void Update() {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "bullet") return;
        active = !active;
    }
}
