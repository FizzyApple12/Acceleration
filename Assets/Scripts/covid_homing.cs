using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class covid_homing : MonoBehaviour {

    public int live = 15000;

    public float speed = 0.075f;

    public GameObject player;

    Vector3 move;

    bool alive = true;

    void Start() {
        Task.Delay(live).ContinueWith(t => End());
    }

    void Update() {
        move = Vector3.Normalize(player.transform.position - transform.position) * speed;
        transform.Translate(move);
        if (!alive) Destroy(gameObject);
    }

    void End() {
        alive = false;
    }
}
