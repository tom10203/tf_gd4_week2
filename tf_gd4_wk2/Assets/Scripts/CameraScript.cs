using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform kart;
    public float smoothTime = .3f;
    public Vector3 offset;
    public Vector3 finishOffset;
    Vector3 vel = Vector3.zero;

    public bool isGameFinished = false;
    public ParticleSystem ps1;
    public ParticleSystem ps2;

    public float spawParticles = 4f;
    public float ps1StartTime = 0.5f;
    public float ps2StartTime = 1f;

    bool playEndGameAudio = true;
    public bool playGame = false;

    private void FixedUpdate()
    {
        if (playGame)
        {
            transform.LookAt(kart.position);

            Vector3 position;
            if (!isGameFinished)
            {
                position = kart.TransformPoint(offset);
            }
            else
            {
                position = kart.TransformPoint(finishOffset);
                ps1.Play();
                ps2.Play();

            }

            transform.position = Vector3.SmoothDamp(transform.position, position, ref vel, smoothTime);

        }
        
    }
}
