using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class Menu : MonoBehaviour

{

    public TextMeshProUGUI totalKillCount;
    public TextMeshProUGUI totalScore;
    public GameObject achievement1;
    public GameObject achievement2;
    public GameObject achievement3;
    
    // Start is called before the first frame update
    void Start()

    {

        //initialise game analytics
        GameAnalytics.Initialize();

        // get total kills and total score and set appropriate text
        totalKillCount.text = PlayerPrefs.GetInt("TotalKills").ToString();
        totalScore.text = PlayerPrefs.GetInt("TotalScore").ToString();
        
        // for the 3 achievements, check if value = 1, if it does show checkmark
        if (PlayerPrefs.GetInt("Achievement1") > 0)

        {
            
            achievement1.SetActive(true);
            
        }
        
        if (PlayerPrefs.GetInt("Achievement2") > 0)

        {
            
            achievement2.SetActive(true);
            
        }
        
        if (PlayerPrefs.GetInt("Achievement3") > 0)

        {
            
            achievement3.SetActive(true);
            
        }

    }

    // identical code 3 times, load scene respective to each button
    public void Level1()

    {

        SceneManager.LoadScene("Level1");

    }
    
    public void Level2()

    {

        SceneManager.LoadScene("Level2");

    }
    
    public void Level3()

    {

        SceneManager.LoadScene("Level3");

    }
    
    // quit application
    public void QuitApplication()

    {
        
        // if in editor, stop play mode by pressing button
        // unnecessary, but I wanted to try out the if statements for specific platforms
        #if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;
        
        // if if any app besides editor, quit application instead
        #else

            Application.Quit();

        #endif

    }
    
}