using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class covid_shoot : MonoBehaviour {


    public GameObject player;
    public float speed = 0.5f;

    Vector3 target = new Vector3(0,0,0);
    bool targeting = true;
    bool alive = true;
    void Start() {
        Task.Delay(3000).ContinueWith(t => Shoot());
    }

    void Update() {
        if (targeting) {
            target = player.transform.position - transform.position;
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, Vector3.Normalize(target) * 1000);
        } else {
            Vector3 move = Vector3.Normalize(target) * speed;
            transform.Translate(move);
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, Vector3.Normalize(target) * 1000);
        }
        if (!alive) Destroy(gameObject);
    }

    void Shoot() {
        targeting = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "kill") return;
        alive = false;
    }
}
