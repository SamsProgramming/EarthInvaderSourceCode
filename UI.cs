using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour

{

    private GameManager gameManagerScript;
    private PlayerController playerControllerScript;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;

    private float startTime;
    private float currentTime;
    
    // Start is called before the first frame update
    void Start()

    {

        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        startTime = Time.time;

    }

    // Update is called once per frame
    void Update()

    {

        currentTime = Time.time - startTime;
        
        healthText.text = "HEALTH: " + playerControllerScript.health.ToString();
        scoreText.text = "SCORE: " + gameManagerScript.score.ToString();

        string minutes = ((int) currentTime / 60).ToString();
        string seconds = (currentTime % 60).ToString("f2");

        if ((currentTime % 60) < 10)

        {

            timerText.text = minutes + ":0" + seconds;

        }

        else

        {

            timerText.text = minutes + ":" + seconds;

        }

    }
    
}