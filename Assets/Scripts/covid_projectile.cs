using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class covid_projectile : MonoBehaviour {

    public float speed = 0.075f;

    public Vector3 direction = new Vector3(0,0,0);

    Vector3 move;

    bool alive = true;

    void Start() {
    }

    void Update() {
        move = Vector3.Normalize(direction) * speed;
        transform.Translate(move);
        if (!alive) Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "kill") return;
        alive = false;
    }

}
