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
    [SerializeField]
    private Text levelName = null;
    [SerializeField]
    private CanvasRenderer[] levelButtonsExceptForIntroLevel = null;
    [SerializeField]
    BallTouchMover ballTouchMover = null;
    [SerializeField]
    private Text timeToCompleteText = null;
    [SerializeField]
    private CanvasRenderer gameOverCanvas = null;
    [SerializeField]
    private CanvasRenderer levelCompleteCanvas = null;
    [SerializeField]
    private CanvasRenderer navigationGroupCanvas = null;
    [SerializeField]
    private CanvasRenderer nextLevelButton = null;
    [SerializeField]
    private CanvasRenderer playButton = null;
    [SerializeField]
    private CanvasRenderer resumeButton = null;
    [SerializeField]
    private CanvasRenderer restartButton = null;
    [SerializeField]
    private CanvasRenderer inGameMenu = null;
    // private
    private float timer = 0f;
    private float startTime = 0f;
    private int thisSceneIndex = 0;
    private int sceneCount = 0;
    private ProgressData progressData = null;

    private void Start()
    {
        if(timeToCompleteText != null)
        {
            timeToCompleteText.text = FormatTime(timeToComplete);
        }
        // set scene name on the dashboard
        if (levelName != null)
        {
            levelName.text = SceneManager.GetActiveScene().name;
        }
        sceneCount = SceneManager.sceneCountInBuildSettings;
        thisSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // get progress data
        progressData = SaveSystem.LoadProgress();
        ManageSaveProgressData();
    }

    private void ManageSaveProgressData()
    {
        // if we are on the main menu
        if (thisSceneIndex == 0)
        {
            if (progressData != null)
            {
                int index = 0;
                while (index < progressData.levels.Length)
                {
                    // enable those levels, which are saved
                    levelButtonsExceptForIntroLevel[index].gameObject.SetActive(progressData.levels[index]);
                    index++;
                }
            }
        }
        // new level - save current level if somehow it's not saved, the 1st index is for intro level, the 0th is the main menu
        if (thisSceneIndex > 1)
        {
            // save already exists
            if (progressData != null)
            {
                // save this level in progress data
                progressData.levels[thisSceneIndex - 2] = true; // one for main menu level another for intro level
                SaveSystem.SaveProgress(progressData);
            }
            // new save
            else
            {
                // one for main menu level another for intro level, that's why -2
                SaveSystem.SaveProgressNew(sceneCount - 2, thisSceneIndex - 2);
            }
        }
    }

    public void StartGame()
    {
        startTime = Time.time;
        InvokeRepeating(nameof(TimeCounter), 0, 1);
        if(ballTouchMover != null)
        {
            ballTouchMover.active = true;
        }
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
        Time.timeScale = 1;
        SceneManager.LoadScene(thisSceneIndex);
    }

    public void GoToSpecificLevel(int buildIndex = -1)
    {
        Time.timeScale = 1;
        // if user did not specify any buildIndex then just load current scene
        if (buildIndex == -1)
            buildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(buildIndex);
    }

    public void NextLevel()
    {
        if (sceneCount == thisSceneIndex + 1)
        {
            SceneManager.LoadScene(0); // if this is the last scene just load main menu
        }
        else
        {
            // make save if progress data doesn't exist
            if(progressData == null)
            {
                SaveSystem.SaveProgressNew(sceneCount - 2, thisSceneIndex - 2);
                progressData = SaveSystem.LoadProgress();
            }
            // make next level available
            progressData.levels[thisSceneIndex - 1] = true; // one for main menu level another for intro level
            SceneManager.LoadScene(thisSceneIndex + 1);
        }
    }

    public void CallEnd()
    {
        if (ballTouchMover != null)
        {
            ballTouchMover.active = false;
        }
        CancelInvoke(nameof(TimeCounter));
        inGameMenu.gameObject.SetActive(false);
        levelCompleteCanvas.gameObject.SetActive(true);
        navigationGroupCanvas.gameObject.SetActive(true);
        nextLevelButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
    }

    private void CallGameOver()
    {
        if (ballTouchMover != null)
        {
            ballTouchMover.active = false;
        }
        CancelInvoke(nameof(TimeCounter));
        CancelInvoke(nameof(TimeCounter));
        inGameMenu.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(true);
        navigationGroupCanvas.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
    }

    private void TimeCounter()
    {
        timer = Time.time - startTime;
        timerText.text = FormatTime(timer) + " / " + FormatTime((float)timeToComplete);
        if(timer > timeToComplete)
        {
            CallGameOver();
        }
        if( (timeToComplete - timer < 10 ) && (timerText.color != Color.red) )
        {
            timerText.color = Color.red;
            timerText.fontStyle = FontStyle.Bold;
        }
    }

    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
