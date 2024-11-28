using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public TextMeshProUGUI lapsText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI endGameTimerText;

    public AudioSource openingAudio;
    public AudioSource mainGameAudio;
    public AudioSource endGameAudio;
    public Camera cam1;
    public Camera cam2;

    public GameObject lapsTimerUIGO;
    public GameObject startGameUIGO;
    public GameObject endGameUIGO;

    public GameObject car2;
    public GameObject camera2Parent;

    float timer = 0f;
    bool startTimer = false;

    private void OnEnable()
    {
        openingAudio.Play();
        
    }

    private void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
        }

        timerText.text = "TIMER : " + (Mathf.Round(timer * 100) / 100f).ToString();

    }
    public void UpdateText(int laps)
    {
        lapsText.text = "LAP : " + laps.ToString() + " / 2";
    }

    public void StartGame()
    {
        CameraScript cameraScript1 = cam1.GetComponent<CameraScript>();
        cameraScript1.playGame = true;

        CameraScript cameraScript2 = cam2.GetComponent<CameraScript>();
        cameraScript2.playGame = true;
        lapsTimerUIGO.SetActive(true);
        startGameUIGO.SetActive(false);
        openingAudio.Stop();
        mainGameAudio.Play();

        startTimer = true;
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

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void TwoPlayerEnable()
    {
        cam1.rect = new Rect(0f, 0f, 0.5f, 1f);

        //cam2.enabled = true;
        camera2Parent.SetActive(true);
        car2.SetActive(true);
    }

    public void OnePlayerEnable()
    {
        cam1.rect = new Rect(0f, 0f, 1f, 1f);

        camera2Parent.SetActive(false);
        car2.SetActive(false);
    }

}
