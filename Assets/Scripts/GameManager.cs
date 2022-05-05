using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // difficulty settings
    public int timeToComplete = 45;
    // references
    [SerializeField]
    private Text timerText = null;
    // private
    private float timer = 0f;
    private float startTime = 0f;
    private int thisSceneIndex = 0;
    private int sceneCount = 0;

    private void Start()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        thisSceneIndex= SceneManager.GetActiveScene().buildIndex;
    }

    public void StartGame()
    {
        startTime = Time.time;
        InvokeRepeating(nameof(TimeCounter), 0, 1);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {

    }

    public void NextLevel()
    {

    }

    public void CallEnd()
    {
        Debug.Log("END REACHED");
    }

    private void CallGameOver()
    {

    }

    private void TimeCounter()
    {
        timer = Time.time - startTime;
        int minutes = (int)timer / 60;
        int seconds = (int)timer - 60 * minutes;
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
