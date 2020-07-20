using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Boss1 : MonoBehaviour {

    public GameObject eventHandler;
    public GameObject player;
    public GameObject endParticles;

    public AudioClip beginning;
    public AudioClip fight;
    public AudioClip end;

    public float health = 100;

    public Vector3 target = new Vector3(0,0,0);
    public GameObject pf_shoot;
    public GameObject pf_homing;
    public GameObject pf_dvd;
    public GameObject pf_projectile;
    public Material warningMaterial;
    public int warningMaterialDelay;

    bool shoot;

    bool dvd;

    bool homing;

    bool bomb;

    bool animating1 = true;

    bool warningMaterialColor = false;
    int warningMaterialCounter = 0;
    bool assignMove = false;

    bool endBoss = true;

    List<GameObject> projectiles = new List<GameObject>();

    void Start() {
        if (AudioManager.Instance != null) AudioManager.Instance.targetVolume = 0;
        player.GetComponent<Player>().bossStart = true;
        player.GetComponent<Player>().boss = true;
        gameObject.GetComponent<Animator>().Play("bossStart");
        gameObject.GetComponent<AudioSource>().Stop();
        gameObject.GetComponent<AudioSource>().loop = false;
        endParticles.GetComponent<ParticleSystem>().Stop();
        //AlertObservers("StartEnd");
    }

    void Update() {
        player.GetComponent<Player>().bossLook = transform.position;
        if (health > 0 && !animating1) {
            //health--;
            if (assignMove) {
                int delay = Random.Range(1000, 5000);
                Task.Delay(delay).ContinueWith(t => assignMove = true);
                computeMove();
                assignMove = false;
            }

            if (bomb) {
                Bomb();
                bomb = false;
            }
            if (dvd) {
                Dvd();
                dvd = false;
            }
            if (homing) {
                Homing();
                homing = false;
            }
            if (shoot) {
                Shoot();
                shoot = false;
            }

            warningMaterialCounter++;

            if (warningMaterialColor) {
                warningMaterial.color = new Color(255, 255, 0, 1);
            } else {
                warningMaterial.color = new Color(0, 0, 0, 1);
            }

            if (warningMaterialCounter >= warningMaterialDelay) {
                warningMaterialColor = !warningMaterialColor;
                warningMaterialCounter = 0;
            }
        } else if (!animating1) {
            if (endBoss) {
                foreach (GameObject projectile in projectiles) {
                    if (projectile != null) Destroy(projectile);
                }
                gameObject.GetComponent<Animator>().Play("bossEnd");
                endParticles.GetComponent<ParticleSystem>().Play();
                endBoss = false;
            }
        }
    }

    void computeMove() {
        int move = Random.Range(0, 4);

        switch (move) {
            case 1:
                bomb = true;
                break;
            case 2:
                shoot = true;
                break;
            case 3:
                homing = true;
                break;
            case 4:
                dvd = true;
                break;
        }
    }

    public void AlertObservers(string message) {
        if (message == "StartStart") {
            gameObject.GetComponent<AudioSource>().clip = beginning;
            gameObject.GetComponent<AudioSource>().Play();
        } else if (message == "StartEnd") {
            animating1 = false;
            player.GetComponent<Player>().bossStart = false;
            player.GetComponent<Player>().bossEnd = true;
            gameObject.GetComponent<Animator>().Play("bossIdle");
            this.transform.position = new Vector3(0, 20, 0);
            gameObject.GetComponent<AudioSource>().Stop();
            gameObject.GetComponent<AudioSource>().clip = fight;
            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<AudioSource>().loop = true;
            int delay = Random.Range(1000, 5000);
            Task.Delay(delay).ContinueWith(t => assignMove = true);
        } else if (message == "EndStart") {
            player.GetComponent<Player>().bossStart = true;
            gameObject.GetComponent<AudioSource>().Stop();
            gameObject.GetComponent<AudioSource>().loop = false;
            gameObject.GetComponent<AudioSource>().clip = end;
            gameObject.GetComponent<AudioSource>().Play();
            endParticles.GetComponent<ParticleSystem>().Play();
        } else if (message == "EndEnd") {
            gameObject.GetComponent<Animator>().StopPlayback();
            player.GetComponent<Player>().bossStart = false;
            player.GetComponent<Player>().bossEnd = true;
            eventHandler.GetComponent<GameEvents>().levelComplete = true;
        }
    }

    // ATTACKS

    public void Bomb() {
        GameObject go = Instantiate(pf_projectile, transform.position, Quaternion.Euler(0,0,0));
        go.GetComponent<covid_projectile>().direction = player.transform.position - transform.position;
        go.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
        projectiles.Add(go);
    }

    public void Shoot() {
        GameObject go = Instantiate(pf_shoot, transform.position, Quaternion.Euler(0,0,0));
        go.GetComponent<covid_shoot>().player = player;
        projectiles.Add(go);
    }

    public void Homing() {
        GameObject go = Instantiate(pf_homing, transform.position, Quaternion.Euler(0,0,0));
        go.GetComponent<covid_homing>().player = player;
        projectiles.Add(go);
    }

    public void Dvd() {
        GameObject go = Instantiate(pf_dvd, transform.position, Quaternion.Euler(0,0,0));
        go.GetComponent<covid_dvd>().direction.x = Random.Range(-1,1);
        go.GetComponent<covid_dvd>().direction.y = Random.Range(-1,1);
        go.GetComponent<covid_dvd>().direction.z = Random.Range(-1,1);
        projectiles.Add(go);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "bullet") {
            health -= Random.Range(1,5);
        }
    }
}
