using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Bullet : MonoBehaviour {

    public float speed = 1f;

    public Vector3 direction = new Vector3(0,0,0);

    Vector3 move;

    bool alive = true;

    void Start() {
        Task.Delay(15000).ContinueWith(t => End());
    }

    void Update() {
        move = Vector3.Normalize(direction) * speed;
        transform.Translate(move);
        if (!alive) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "bav") return;
        speed = 0;
        GetComponent<ParticleSystem>().Play();
        //End();
    }

    void End() {
        alive = false;
    }

}
