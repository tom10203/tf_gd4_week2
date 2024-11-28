using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointsManager : MonoBehaviour
{
    public Transform[] checkpoints;
    public GameObject car1;
    public GameObject car2;
    Car carScript1;
    Car carScript2;
    public CameraScript cam1;
    public CameraScript cam2;


    [HideInInspector]
    public int i = 0;
    Transform startCheckpoint;
    public int lapNumber = 0;
    public int totalLaps = 1;
    bool isFirst = true;
    MeshRenderer startTransformMeshRenderer;

    public GameObject gameUIManagerGO;
    GameUIManager gameUIManager;

    private void Start()
    {
        startCheckpoint = checkpoints[0];
        startTransformMeshRenderer = startCheckpoint.GetComponent<MeshRenderer>();
        startTransformMeshRenderer.enabled = false;

        carScript1 = car1.GetComponent<Car>();
        carScript2 = car2.GetComponent<Car>();
        gameUIManager = gameUIManagerGO.GetComponent<GameUIManager>();
    }
    public void VerifyCheckpoint(Transform checkpoint)
    {
        if (checkpoint == startCheckpoint && checkpoints[i] == startCheckpoint)
        {
            if (isFirst)
            {
                isFirst = false;
                startTransformMeshRenderer.enabled = true;  
            }
            else
            {
     
                lapNumber++;
                gameUIManager.UpdateText(lapNumber);


                if (lapNumber == totalLaps)
                {
                    CameraScript camScript1 = cam1.GetComponent<CameraScript>();
                    CameraScript camScript2 = cam2.GetComponent<CameraScript>();

                    carScript1.isGameFinished = true;
                    camScript1.isGameFinished = true;

                    carScript2.isGameFinished = true;
                    camScript2.isGameFinished = true;

                    gameUIManager.EndGame();

                    // Adding below line so gameUIManager.EndGame() is only called once
                    lapNumber++;        
                }
            }
        }
        if (checkpoints[i] == checkpoint)
        {
            MoveI();
        }
    }

    public void MoveI()
    {
        i = (i+1) % checkpoints.Length;
    }
}
