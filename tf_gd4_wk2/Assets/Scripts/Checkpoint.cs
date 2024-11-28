using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    CheckpointsManager checkpointsManager;
    public AudioSource passThroughCheckpoint;


    private void Start()
    {
        checkpointsManager = transform.parent.GetComponent<CheckpointsManager>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 9)
        {
            passThroughCheckpoint.Play();
            checkpointsManager.VerifyCheckpoint(transform);
        }
        
    }

}
