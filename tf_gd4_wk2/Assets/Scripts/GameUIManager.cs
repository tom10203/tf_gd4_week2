using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public TextMeshProUGUI lapsText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI endGameTimerText;

    public AudioSource openingAudio;
    public AudioSource mainGameAudio;
    public AudioSource endGameAudio;
    public Camera cam;

    public GameObject lapsTimerUIGO;
    public GameObject startGameUIGO;
    public GameObject endGameUIGO;

    float timer = 0f;

    private void OnEnable()
    {
        openingAudio.Play();
        
    }


    private void Update()
    {
        timer += Time.deltaTime;
        
        timerText.text = "TIMER : " + (Mathf.Round(timer * 100) / 100f).ToString();
    }
    public void UpdateText(int laps)
    {
        lapsText.text = "LAP : " + laps.ToString() + " / 3";
    }

    public void StartGame()
    {
        CameraScript cameraScript = cam.GetComponent<CameraScript>();
        cameraScript.playGame = true;
        lapsTimerUIGO.SetActive(true);
        startGameUIGO.SetActive(false);
        openingAudio.Stop();
        mainGameAudio.Play();

    }

    public void EndGame()
    {
        mainGameAudio.Stop();
        endGameAudio.Play();
        lapsTimerUIGO.SetActive(false);

        float newTimer = timer;
        endGameUIGO.SetActive(true);
        endGameTimerText.text = "YOUR TIME : " + newTimer.ToString();

    }

}
