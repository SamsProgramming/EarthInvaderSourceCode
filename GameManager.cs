using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Setup;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour

{

    // stats
    public int score;
    public int totalScore;
    public int totalKills;

    // achievements
    public int achievement1;
    public int achievement2;
    public int achievement3;

    public GameObject level;
    public float gameSpeed;
    public bool stopLevel = false;

    public PlayerController playerControllerScript;
    public int scoreToAdd = 0;
    
    // mobile controls
    public GameObject arrowLeft;
    public GameObject arrowUp;
    public GameObject arrowRight;
    public GameObject arrowDown;
    public GameObject shootButton;
    public GameObject menuButton;

    public GameObject levelCompleteObject;
    
    // scene variable for getting the name
    private Scene scene;

    void Start()

    {
        
        GameAnalytics.Initialize();
        
        // if the game is running on android, enable mobile controls
        #if UNITY_ANDROID
        
        arrowLeft.SetActive(true);
        arrowUp.SetActive(true);
        arrowRight.SetActive(true);
        arrowDown.SetActive(true);
        shootButton.SetActive(true);
        menuButton.SetActive(true);
        
        #endif

        // get player controller script
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        // create a new progression event when a level is started, with the name of the level
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, SceneManager.GetActiveScene().name);

    }
    
    // Update is called once per frame
    void Update()
    
    {

        // move level forward if not stopped
        if (stopLevel == false)

        {
            
            level.transform.Translate(Vector3.left * gameSpeed * Time.deltaTime);
            
        }

        // if total score is more than 10,000
        if (PlayerPrefs.GetInt("TotalScore") > 10000)

        {

            // unlock achievement 1
            PlayerPrefs.SetInt("Achievement1", 1);
            
        }
        
        // if total kills is more than 100
        if (PlayerPrefs.GetInt("TotalKills") > 100)

        {

            // unlock achievement 2
            PlayerPrefs.SetInt("Achievement2", 1);
            
        }

        // if the level stopped and there are no objects with tags enemy or boss, then finish the level
        if (GameObject.FindWithTag("Enemy") == false && GameObject.FindWithTag("Boss") == false && stopLevel == true)

        {

            FinishLevel();

        }
        
    }

    public void StopLevel()

    {

        stopLevel = true;

    }

    public void FinishLevel()

    {
        
        // enable text and start timer to return to menu
        levelCompleteObject.SetActive(true);
        Invoke("ReturnToMenu", 1.0f);
        // create a progression event when the level is complete, with the name of the current level and the total score
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, SceneManager.GetActiveScene().name, totalScore);
        // do it again but show the kills this time, the function only takes one int so I did it twice
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, SceneManager.GetActiveScene().name, totalKills);

    }

    private void ReturnToMenu()

    {
        
        playerControllerScript.ReturnToMenu();
        
    }

    // identical code for 7 methods, get score to add per enemy and add to total score, if the player has the score upgrade, the score to add is doubled

    public void KamikazeScore()

    {

        if (playerControllerScript.scoreUpgrade == true)

        {

            score = score + (100 * 2);
            scoreToAdd = 200;

        }

        else

        {

            score = score + 100;
            scoreToAdd = 100;

        }

        totalScore = PlayerPrefs.GetInt("TotalScore") + scoreToAdd;
        PlayerPrefs.SetInt("TotalScore", totalScore);

    }
    
    public void APCScore()

    {

        if (playerControllerScript.scoreUpgrade == true)

        {

            score = score + (200 * 2);
            scoreToAdd = 400;

        }

        else

        {

            score = score + 200;
            scoreToAdd = 200;

        }

        totalScore = PlayerPrefs.GetInt("TotalScore") + scoreToAdd;
        PlayerPrefs.SetInt("TotalScore", totalScore);

    }
    
    public void FighterScore()

    {

        if (playerControllerScript.scoreUpgrade == true)

        {

            score = score + (300 * 2);
            scoreToAdd = 600;

        }

        else

        {

            score = score + 300;
            scoreToAdd = 300;

        }

        totalScore = PlayerPrefs.GetInt("TotalScore") + scoreToAdd;
        PlayerPrefs.SetInt("TotalScore", totalScore);

    }
    
    public void TankScore()

    {

        if (playerControllerScript.scoreUpgrade == true)

        {

            score = score + (200 * 2);
            scoreToAdd = 400;

        }

        else

        {

            score = score + 200;
            scoreToAdd = 200;

        }

        totalScore = PlayerPrefs.GetInt("TotalScore") + scoreToAdd;
        PlayerPrefs.SetInt("TotalScore", totalScore);

    }
    
    public void UFOScore()

    {

        if (playerControllerScript.scoreUpgrade == true)

        {

            score = score + (300 * 2);
            scoreToAdd = 600;

        }

        else

        {

            score = score + 300;
            scoreToAdd = 300;

        }

        totalScore = PlayerPrefs.GetInt("TotalScore") + scoreToAdd;
        PlayerPrefs.SetInt("TotalScore", totalScore);

    }
    
    public void BomberScore()

    {

        if (playerControllerScript.scoreUpgrade == true)

        {

            score = score + (300 * 2);
            scoreToAdd = 600;

        }

        else

        {

            score = score + 300;
            scoreToAdd = 300;

        }

        totalScore = PlayerPrefs.GetInt("TotalScore") + scoreToAdd;
        PlayerPrefs.SetInt("TotalScore", totalScore);

    }

    public void FinalBossScore()

    {
        
        if (playerControllerScript.scoreUpgrade == true)

        {

            score = score + (1000 * 2);
            scoreToAdd = 2000;

        }

        else

        {

            score = score + 1000;
            scoreToAdd = 1000;

        }

        totalScore = PlayerPrefs.GetInt("TotalScore") + scoreToAdd;
        PlayerPrefs.SetInt("TotalScore", totalScore);
        
    }

    // add one to kill count
    public void IncreaseKillCount()

    {

        totalKills++;
        PlayerPrefs.SetInt("TotalKills", totalKills);

    }

}