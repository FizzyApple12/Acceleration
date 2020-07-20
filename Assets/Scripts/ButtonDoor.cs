using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour {

    public GameObject button;
    public Vector3 deltaPos;
    public bool inverted;
    public float speed;
    Vector3 startingPos;
    bool active = false;

    void Start() {
        startingPos = transform.position;
    }

    void Update() {
        Vector3 goal;
        if (active) goal = startingPos + deltaPos;
        else goal = startingPos;

        transform.position = Vector3.MoveTowards(transform.position, goal, speed);

        if (inverted) active = !button.GetComponent<ShootButton>().active;
        else active = button.GetComponent<ShootButton>().active;
    }
}
