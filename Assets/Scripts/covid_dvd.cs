using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class covid_dvd : MonoBehaviour {
    public float speed = 0.275f;

    public Vector3 direction = new Vector3(0,0,0);

    public int iteration =  0;

    public GameObject pf_dvd;

    Vector3 move;

    public bool alive = true;

    bool canDupe = false;

    int bounces = 0;

    void Start() {
        Task.Delay(1000).ContinueWith(t => readyDupe());
    }

    void readyDupe() {
        canDupe = true;
    }

    void Update() {
        move = Vector3.Normalize(direction) * speed;
        transform.Translate(move);
        if (!alive) Destroy(gameObject);
    }
    
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "kill") {
            Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
            return;
        }

        bounces++;

        // if (canDupe && iteration < 3) {
        //     GameObject go = Instantiate(pf_dvd, transform.position, Quaternion.Euler(0,0,0));
        //     go.GetComponent<covid_dvd>().iteration = iteration + 1;
        //     go.GetComponent<covid_dvd>().direction = other.GetContact(0).normal;
        //     canDupe = false;
        //     Task.Delay(1000).ContinueWith(t => readyDupe());
        //     //go.GetComponent<covid_dvd>().direction = other.GetContact(0).normal;
        // }

        if (bounces > 5) alive = false;

        direction = Vector3.Reflect(direction, other.GetContact(0).normal);

        speed += 0.01f;
    }
}
