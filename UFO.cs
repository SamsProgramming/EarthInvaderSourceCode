using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour

{

    public float speed;
    public int health;
    public bool isFiring;
    public GameObject projectile;
    public GameObject barrel;

    public int playerDamage;
    private GameObject player;
    private PlayerController playerControllerScript;
    public GameObject smokeParticles;
    public GameObject explosionParticles;
    private GameManager gameManagerScript;

    private bool isDestroyed = false;

    // Start is called before the first frame update
    void Start()
    
    {

        // get player and game manager script
        player = GameObject.Find("Player");
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
 
    {

        // if alive and not firing
        if (health > 0)

        {

            if (!isFiring)

            {

                // follow player on x axis
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), (speed * Time.deltaTime));

            }

            // if x matches players
            if (transform.position.x == player.transform.position.x)

            {

                // and isnt firing, start firing
                // during this period, the ufo will fire its 3 bomb burst and stop moving, letting the player move up and attack it
                if (!isFiring)

                {

                    StartCoroutine("Fire");

                }

            }

        }

        // move downwards when damaged until it crashes to the ground or is destroyed by the player
        else if (health == 0)

        {

            transform.Translate(Vector3.down * (speed / 2) * Time.deltaTime);
            smokeParticles.SetActive(true);

        }

        // destroy
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
        
        // destroy the ufo if it's offscreen
        if (transform.position.y < -4.5)

        {
            
            Destroy(gameObject);

        }

    }

    IEnumerator Fire()

    {

        // again, fire burst of 3 bombs during which ufo doesnt move
        // done using a while loop and a yield return inside
        isFiring = true;

        int burst = 0;

        while (burst < 3)

        {

            Instantiate(projectile, barrel.transform.position, barrel.transform.rotation);
            burst++;
            yield return new WaitForSeconds(0.5f);

        }

        isFiring = false;

    }
    
    void DestroyObject()

    {
        
        // add counters and destroy
        gameManagerScript.UFOScore();
        gameManagerScript.IncreaseKillCount();
        Destroy(gameObject);
        
    }
    
    private void OnCollisionEnter(Collision collision)
    
    {

        // ignore other enemies for collisions
        if (collision.gameObject.CompareTag("Enemy"))

        {
            
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());

        }


        // take damage and destroy player projectile
        if (collision.gameObject.CompareTag("FriendlyProjectile"))

        {

            playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
            playerDamage = playerControllerScript.damage;
            health = health - playerDamage;
            Destroy(collision.gameObject);

        }
        
    }

}