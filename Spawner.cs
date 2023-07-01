using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour

{

    public GameObject[] objectList;
    public bool spawnBoss = false;

    private GameManager gameManagerScript;

    private void OnTriggerEnter(Collider collider)
    
    {

        // if spawn boss is set to false
        if (spawnBoss == false)

        {

            // if colliding with player
            if (collider.gameObject.CompareTag("Player"))

            {

                // destroy the box collider to prevent further triggers
                Destroy(gameObject.GetComponent<BoxCollider>());
                
                // iterate through array of objects and instantiate
                for (int i = 0; i < objectList.Length; i++)

                {

                    Instantiate(objectList[i].gameObject);

                }
                
                // destroy object when done
                Destroy(gameObject);
            }

        }

        else

        {
            
            // if colliding with player and spawn boss is true
            if (collider.gameObject.CompareTag("Player"))
            
            {
                
                // destroy collider again
                Destroy(gameObject.GetComponent<BoxCollider>());
                // spawn boss
                spawnBoss = true;
                // get the game manager and stop the level so that the level doesnt leave the screen
                gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
                gameManagerScript.StopLevel();
                // spawn the boss, element 0 because I set this to only have one object to spawn
                Instantiate(objectList[0].gameObject);
                // destroy when done
                Destroy(gameObject);
                                
            }

        }

    }
    
}