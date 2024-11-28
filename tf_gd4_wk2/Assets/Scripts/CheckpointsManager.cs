using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointsManager : MonoBehaviour
{
    public Transform[] checkpoints;
    public GameObject car;
    Car carScript;
    public CameraScript cam;


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

        carScript = car.GetComponent<Car>();
        gameUIManager = gameUIManagerGO.GetComponent<GameUIManager>();
    }
    public void VerifyCheckpoint(Transform checkpoint)
    {
        if (checkpoint == startCheckpoint && checkpoints[i] == startCheckpoint)
        {
            print($"startCheckpoint crossed");
            if (isFirst)
            {
                isFirst = false;
                startTransformMeshRenderer.enabled = true;  
            }
            else
            {
     
                lapNumber++;
                gameUIManager.UpdateText(lapNumber);
                //  Update UI


                if (lapNumber == totalLaps)
                {
                    print($"Game End");
                    CameraScript camScript = cam.GetComponent<CameraScript>();  
                    carScript.isGameFinished = true;
                    camScript.isGameFinished = true;
                    gameUIManager.EndGame();

                    // Adding below line so gameUIManager.EndGame() is only called once
                    lapNumber++;
                    
                    
                }
            }
        }
        if (checkpoints[i] == checkpoint)
        {
            MoveI();
            print($"Next checkpoint is {i}");
        }
    }

    public void MoveI()
    {
        i = (i+1) % checkpoints.Length;
    }
}
