using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour {

    public GameObject GameEvents;

    public GameObject head;
    public GameObject body;
    public GameObject gun;
    public GameObject bullet;
    public GameObject barrelEnd;
    public GameObject gunDisplay;

    public List<Material> worldMaterials;

    public List<string> gunModes;
    public int maxGunMode;
    public int currentGunMode = 0;

    public float maxVelocityChange = 10.0f;
    public float speed = 1.0f;
    public float jumpSpeed = 0.1f;
    public float gravity = 9.81f;
    public float slideSpeed = 0.0f;
    public float grappleSpeed = 25.0f;

    public float rotationX = 0.0f;
    public float rotationY = 0.0f;
    public float lookSpeed = 0.0f;

    public bool paused = false;
    public bool allowUnpause = true;
    public bool bossStart = false;
    public bool bossEnd = false;
    public Vector3 bossLook = new Vector3(0,0,0);
    public bool previousFramePaused = false;

    public bool boss;


    public float currentWorldTransparancy = 1;

    public bool touchingFloor = false;
    public bool touchingWall = false;
    public bool wallSide = false; // false:left, true:right
    public float zRotation = 0.0f;
    public float xRotation = 0.0f;

    public bool sliding = false;
    public bool grappling = false;

    public bool undo = false;
    public Vector3 grappleForce = new Vector3(0,0,0);
    public RaycastHit grappleTo;
    public Vector3 gravityDirection;
    public Vector3 lastSteppable;

    Rigidbody rigidBody;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
        if (StatManager.Instance != null) StatManager.Instance.startRecording(gameObject);
    }

    void Update() {

        if (bossStart) {
            paused = true;
            allowUnpause = false;
            rotationX = 0;
            rotationY = 0;
            head.transform.LookAt(bossLook);
        }

        if (bossEnd) {
            paused = false;
            allowUnpause = true;
            bossEnd = false;
            head.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (maxGunMode > -1) {
            gunDisplay.GetComponent<TextMeshPro>().text = gunModes[currentGunMode];
            gun.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Q) && !paused) currentGunMode--;
            if (Input.GetKeyDown(KeyCode.E) && !paused) currentGunMode++;
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)) resetWeapons();
            if (currentGunMode < 0) currentGunMode = maxGunMode;
            if (currentGunMode > maxGunMode || currentGunMode > gunModes.Count - 1) currentGunMode = 0;

            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !paused) {
                switch (currentGunMode) {
                    case 0:
                        rigidBody.detectCollisions = transform.position.y < -9;
                        currentWorldTransparancy -= 0.1f;
                        break;
                    case 1:
                        if (!grappling && Physics.Raycast(head.transform.position, head.transform.forward, out grappleTo, Mathf.Infinity)) {
                            grappling = true;
                        }
                        break;
                    case 2:
                        undo = true;
                        break;
                    case 3:
                        GameObject go = Instantiate(bullet, barrelEnd.transform.position, barrelEnd.transform.rotation);
                        go.GetComponent<Bullet>().direction = Vector3.forward;
                        break;
                }
            } else if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && !paused) {
                switch (currentGunMode) {
                    case 0:
                        rigidBody.detectCollisions = transform.position.y < -9;
                        currentWorldTransparancy -= 0.1f;
                        break;
                    case 1:
                        if (!grappling && Physics.Raycast(head.transform.position, head.transform.forward, out grappleTo, Mathf.Infinity)) {
                            grappling = true;
                        }
                        break;
                    case 2:
                        undo = true;
                        break;
                    case 3:
                        break;
                }
            } else {
                resetWeapons();
            }
            if (grappling) {
                grappleForce = Vector3.Normalize(grappleTo.point - transform.position) * grappleSpeed;
            } else {
                grappleForce = new Vector3(0,0,0);
            }
            
            currentWorldTransparancy = Mathf.Clamp(currentWorldTransparancy, 0.15f, 1);
            foreach (Material material in worldMaterials) {
                Color c = material.GetColor("_Color");
                c.a = currentWorldTransparancy;
                material.SetColor("_Color", c);
            }
        } else {
            gun.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && allowUnpause) paused = !paused;

        if (paused != previousFramePaused) {
            previousFramePaused = paused;
            if (paused) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (!paused) {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationY += Input.GetAxis("Mouse X") * lookSpeed;
        }

        rotationX = Mathf.Clamp(rotationX, -90, 90);

        if (!touchingFloor && touchingWall && !paused) {
            zRotation += ((wallSide) ? 1 : -1);
            zRotation = Mathf.Clamp(zRotation, -15, 15);
            rigidBody.AddForce(gravityDirection * rigidBody.mass);
        } else {
            zRotation -= Mathf.Clamp(zRotation, -1, 1);
            rigidBody.AddForce(Vector3.down * gravity * rigidBody.mass);
        }

        if (touchingFloor && Input.GetKeyDown(KeyCode.LeftShift) && !paused && !sliding) {
            rigidBody.AddForce(transform.forward * slideSpeed, ForceMode.VelocityChange);
            sliding = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !paused && sliding) {
            sliding = false;
            touchingFloor = true;
        }

        if (!bossStart) {
            if (sliding) {
                gameObject.GetComponent<CapsuleCollider>().height = 0.5f;
                head.transform.localPosition = new Vector3(0, 0.1f, 0);
            } else {
                gameObject.GetComponent<CapsuleCollider>().height = 2f;
                head.transform.localPosition = new Vector3(0, 0.5f, 0);
            }

            head.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            body.transform.localRotation = Quaternion.Euler(0, rotationY, zRotation);
        }

        if (!paused && !sliding && !grappling) {
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	        targetVelocity = transform.TransformDirection(targetVelocity);
	        targetVelocity *= speed;
	        Vector3 velocity = rigidBody.velocity;
	        Vector3 velocityChange = (targetVelocity - velocity);
	        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
	        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
	        velocityChange.y = 0;
	        rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);
	        if ((touchingFloor || touchingWall) && Input.GetButtonDown("Jump")) {
	            rigidBody.velocity = velocity + jumpSpeed * transform.up;
	        }
        }
        if (grappling) {
            rigidBody.AddForce(grappleForce);
            gun.GetComponent<LineRenderer>().SetPosition(1, grappleTo.point);
            gun.transform.LookAt(grappleTo.point);
        } else {
            gun.GetComponent<LineRenderer>().SetPosition(1, barrelEnd.transform.position);
            gun.transform.rotation = head.transform.rotation;
        }
        gun.GetComponent<LineRenderer>().SetPosition(0, barrelEnd.transform.position);

        if (touchingFloor || touchingWall) lastSteppable = transform.position;
    }

    private void resetWeapons() {
        rigidBody.detectCollisions = true;
        currentWorldTransparancy += 0.1f;
        grappling = false;
        undo = currentGunMode == 2;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "floor") touchingFloor = true;
        else if (other.gameObject.tag == "wall") {
            touchingWall = true;
            Vector3 sum = transform.right + other.GetContact(0).normal;
            wallSide = sum.x > -1 && sum.x < 1;
            gravityDirection = -other.GetContact(0).normal * gravity;
        }
        else if (other.gameObject.tag == "kill" || transform.position.y < -10) {
            if (undo) transform.position = lastSteppable + new Vector3(0, 1, 0);
            else GameEvents.SendMessage("FailLevel");
        }
        else if (other.gameObject.tag == "goal") GameEvents.SendMessage("CompleteLevel");
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "floor") touchingFloor = false;
        else if (other.gameObject.tag == "wall") touchingWall = false;
    }
}
