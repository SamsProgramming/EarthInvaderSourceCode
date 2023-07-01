using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APC : MonoBehaviour

{
   
    public int health;
    public float speed;
    public float shotDelay;
    public GameObject barrel;
    public GameObject projectile;

    public int playerDamage;
    private GameObject player;
    private PlayerController playerControllerScript;
    public GameObject smokeParticles;
    public GameObject explosionParticles;

    private bool isDestroyed = false;
    private GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()

    {

        // find player and get game manager script from game manager
        player = GameObject.Find("Player");
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        // start invoke repeating to fire projectiles
        InvokeRepeating("Fire", 2.0f, shotDelay);

    }

    // Update is called once per frame

    void Update()

    {

        // if more than zero health
        if (health > 0)

        {

            // try matching players position
            if (transform.position.x < player.transform.position.x)

            {

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), ((speed + 1) * Time.deltaTime));

            }

            // if the player is missed, then drive off
            else if (transform.position.x > player.transform.position.x)

            {

                transform.Translate(Vector3.left * speed * Time.deltaTime);

            }

        }

        // if zero health, enter damaged state
        else if (health == 0)

        {

            transform.Translate(Vector3.right * (speed / 2) * Time.deltaTime);
            CancelInvoke("Fire");
            smokeParticles.SetActive(true);

        }

        // else, destroy object
        else

        {

            if (isDestroyed == false)

            {

                isDestroyed = true;
                Destroy(gameObject.GetComponent<BoxCollider>());
                explosionParticles.SetActive(true);
                Invoke("DestroyObject", 1.0f);

            }

        }

        // destroy the apc if it's offscreen
        if (transform.position.x > 12)

        {

            Destroy(gameObject);

        }

    }
    
    void Fire()

    {

        // instantiate projectile from barrel
        Instantiate(projectile, barrel.transform.position, barrel.transform.rotation);

    }
    
    // add score, kill count and destroy object
    void DestroyObject()

    {
        
        gameManagerScript.APCScore();
        gameManagerScript.IncreaseKillCount();
        Destroy(gameObject);
        
    }
    
    private void OnCollisionEnter(Collision collision)
    
    {

        // ignore collisions with other enemies
        if (collision.gameObject.CompareTag("Enemy"))

        {
            
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());

        }

        // take damage and destroy players projectiles
        if (collision.gameObject.CompareTag("FriendlyProjectile"))

        {

            playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
            playerDamage = playerControllerScript.damage;
            health = health - playerDamage;
            Destroy(collision.gameObject);

        }

    }
    
}