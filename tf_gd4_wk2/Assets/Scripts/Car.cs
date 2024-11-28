using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    [Header("Main Componenets")]
    public int playerNumber;
    public Transform kart;
    public Transform sphere;
    public Transform spawnPoint;
    public GameObject boostPS;
    public GameObject driftPS;
    Rigidbody sphereRB;
    string verticalInput;
    string horizontalInput;
    KeyCode driftButton;

    [Header("Ground Move Speed Variables")]
    public float moveSpeed, moveSpeedT;
    float speed;

    [Header("Drift Variables")]
    public float boostForce;
    public float timeInbetweenBoost = 1f;
    public float boostSpeed;
    public float boostTime = 2f;
    float boostTimer;
    bool boost = false;
    bool drifting = false;
    float driftAngle = 0f;
    Vector3 forceToAddinDrift;


    public Vector3 offset;

    [Header("Rotation Variables")]
    public float turnSpeed;
    public float groundAlignmentSpeed = 5f;
    float dir;

    [Header("Air Move Speed Variables")]
    public float gravity = 10f;
    public float dragFactor = 0.1f;

    [HideInInspector]
    public bool isGameFinished;

    void Start()
    {
        sphereRB = sphere.GetComponent<Rigidbody>();
        if (playerNumber == 1)
        {
            driftButton = KeyCode.L;
            verticalInput = "Vertical1";
            horizontalInput = "Horizontal1";
        }
        else
        {
            driftButton = KeyCode.Space;
            verticalInput = "Vertical2";
            horizontalInput = "Horizontal2";
        }
    }

    void Update()
    {
        kart.position = sphere.position + offset;

        if (!isGameFinished)
        {
            float horizontalInput = Input.GetAxis(this.horizontalInput);
            float verticalInput = Input.GetAxis(this.verticalInput);

            if (Input.GetKeyDown(KeyCode.P))
            {
                // Incase a car gets stuck
                SceneManager.LoadScene(0);
            }

            if (verticalInput != 0)
            {
                speed = Mathf.Lerp(speed, verticalInput * moveSpeed, moveSpeedT);
            }
            else
            {
                speed = 0f;
            }


            RaycastHit hit;
            if (Physics.Raycast(kart.position, Vector3.down, out hit, 1f, 1 << 6))
            {
                Quaternion targetRotation = Quaternion.FromToRotation(kart.up, hit.normal) * kart.rotation;
                kart.rotation = Quaternion.Slerp(kart.rotation, targetRotation, groundAlignmentSpeed * Time.deltaTime);
                sphereRB.drag = 3f;


                if (Input.GetKeyDown(driftButton) && horizontalInput != 0 && !boost && verticalInput > 0)
                {
                    drifting = true;
                    driftPS.SetActive(true);
                    dir = horizontalInput > 0 ? 1 : -1;
                    driftAngle = 0f;
                }

            }
            else
            {
                sphereRB.AddForce(Vector3.down * gravity * 1000f * Time.deltaTime);
                sphereRB.drag = dragFactor;
                speed = 0f;
            }


            if (Input.GetKeyUp(driftButton))
            {
                if (drifting)
                {
                    boost = true;
                    boostTimer = 0f;
                    speed = speed + boostSpeed;

                }
                drifting = false;
            }

            if (horizontalInput != 0)
            {
                if (drifting)
                {
                    horizontalInput += Mathf.Abs(horizontalInput) * dir;
                }
                Steer(horizontalInput);
            }

            if (boost)
            {
                boostPS.SetActive(true);
                driftPS.SetActive(false);
                if (boostTimer > boostTime)
                {
                    boost = false;
                }
                boostTimer += Time.deltaTime;
                speed = Mathf.Lerp(speed, moveSpeed + boostForce, Time.deltaTime);
            }
            else
            {
                boostPS.SetActive(false);
            }
        }


        else
        {
            sphereRB.velocity = Vector3.Lerp(sphereRB.velocity, Vector3.zero, Time.deltaTime);  
        }

    }

    void Steer(float direction)
    {
        float degreesToRotate = direction * turnSpeed * Time.deltaTime * 1000f;
        if (drifting)
        {
            driftAngle -= degreesToRotate;
            driftAngle = Mathf.Clamp(driftAngle, -90, 90);
            forceToAddinDrift = Quaternion.Euler(0, driftAngle, 0) * kart.forward;
        }
        kart.Rotate(Vector3.up, degreesToRotate, Space.Self);
    }

    private void FixedUpdate()
    {
        if (!isGameFinished)
        {
            if (Mathf.Abs(speed) > 0)
            {
                if (drifting)
                {
                    sphereRB.AddForce(forceToAddinDrift * speed, ForceMode.Acceleration);
                }
                else
                {
                    sphereRB.AddForce(kart.forward * speed, ForceMode.Acceleration);
                }

            }

        }
    }
}
