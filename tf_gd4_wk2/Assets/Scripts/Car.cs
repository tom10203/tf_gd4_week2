using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Car : MonoBehaviour
{
    public Transform kart;
    public Transform sphere;
    public Transform spawnPoint;
    Rigidbody sphereRB;
    public float turnSpeed;
    public float moveSpeed, moveSpeedT;
    [HideInInspector]
    float speed;
    public Vector3 offset;
    public float groundAlignmentSpeed = 5f;
    public float gravity = 10f;
    public float dragFactor = 0.1f;
    bool drifting = false;
    float dir;
    float driftAngle = 0f;

    public GameObject blps;
    public GameObject brps;
    public ParticleSystem bl;
    public ParticleSystem br;
    public float psXVelocity = 2f;


    Vector3 forceToAddinDrift;
    Vector3 currentDrift;

    bool boost;
    public float boostForce;
    public float rayDist = 1f;

    public bool isGameFinished;
    void Start()
    {
        sphereRB = sphere.GetComponent<Rigidbody>();
    }

    void Update()
    {
        kart.position = sphere.position + offset;

        if (!isGameFinished)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");


            if (Input.GetKeyDown(KeyCode.P))
            {
                sphere.position = spawnPoint.position;
                //kart.position = spawnPoint.position;
            }

            if (verticalInput != 0)
            {
                speed = Mathf.Lerp(speed, verticalInput * moveSpeed * Time.deltaTime * 1000f, moveSpeedT);
            }
            else
            {
                speed = 0f;
            }


            RaycastHit hit;
            Debug.DrawRay(sphere.position, Vector3.down * rayDist, Color.red);
            if (Physics.Raycast(kart.position, Vector3.down, out hit, 1f, 1 << 6))
            {
                // Create a rotation that combines ground normal and current rotation

                Quaternion targetRotation = Quaternion.FromToRotation(kart.up, hit.normal) * kart.rotation;
                kart.rotation = Quaternion.Slerp(kart.rotation, targetRotation, groundAlignmentSpeed * Time.deltaTime);
                sphereRB.drag = 3f;


                if (Input.GetKeyDown(KeyCode.Space) && horizontalInput != 0)
                {
                    drifting = true;
                    dir = horizontalInput > 0 ? 1 : -1;
                    driftAngle = 0f;

                    blps.SetActive(true);
                    brps.SetActive(true);

                    var blvel = bl.velocityOverLifetime;
                    blvel.x = psXVelocity * dir;

                    var brvel = br.velocityOverLifetime;
                    brvel.x = psXVelocity * dir;

                }

            }
            else
            {
                sphereRB.AddForce(Vector3.down * gravity * 1000f * Time.deltaTime);
                sphereRB.drag = dragFactor;
                speed = 0f;
            }


            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (drifting)
                {
                    boost = true;
                }
                drifting = false;
                blps.SetActive(false);
                brps.SetActive(false);
            }

            if (horizontalInput != 0)
            {
                if (drifting)
                {
                    horizontalInput += Mathf.Abs(horizontalInput) * dir;
                }
                Steer(horizontalInput);
            }

            //print($"kart.forward {kart.forward}")
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

            if (boost)
            {
                sphereRB.AddForce(kart.forward * boostForce, ForceMode.Acceleration);
                boost = false;
            }
        }
    }


}
