using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour

{

    public PlayerController playerControllerScript;

    void Start()

    {

        // get player controller script
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    void Update()
 
    {

        // move left
        transform.Translate(Vector3.left * 1 * Time.deltaTime);

        // if the powerup is off screen, set its position to the other side so that the player can get it if they missed it
        if (transform.position.x < -12)

        {
            
            transform.position = new Vector3(12.0f, transform.position.y, transform.position.z);
            
        }

    }

    private void OnTriggerEnter(Collider collision)
    
    {

        // if colliding with player, call upgrade method in player script and destroy
        if (collision.CompareTag("Player"))

        {
            
            playerControllerScript.GetUpgrade();
            Destroy(gameObject);

        }
    
    }

}