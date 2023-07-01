using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour

{

    public int health;
    public float speed;
    public float shotDelay;
    public GameObject barrel;
    public GameObject projectile;

    public Vector3 startPosition;
    private bool movingUp = true;

    public int playerDamage;
    private PlayerController playerControllerScript;
    public GameObject smokeParticles;
    public GameObject explosionParticles;

    private bool isDestroyed = false;
    private GameManager gameManagerScript;
    
    // Start is called before the first frame update
    void Start()

    {

        // save start position, get game manager script and start firing
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        InvokeRepeating("Fire", 2.0f, shotDelay);
        startPosition = transform.position;

    }

    // Update is called once per frame

    void Update()

    {

        // if alive
        if (health > 0)

        {

            // go forward and move up and down
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            // up and down is based on starting position
            if (movingUp == true)

            {

                transform.Translate(Vector3.up * (speed / 2) * Time.deltaTime);

            }

            if (movingUp == false)

            {

                transform.Translate(Vector3.down * (speed / 2) * Time.deltaTime);

            }

            if (transform.position.y > startPosition.y + 1.0f)

            {

                movingUp = false;

            }

            if (transform.position.y < startPosition.y - 1.0f)

            {

                movingUp = true;

            }

        }

        // in damaged state, stop firing and keep moving left
        else if (health == 0)

        {

            transform.Translate(Vector3.forward * (speed / 2) * Time.deltaTime);
            CancelInvoke("Fire");
            smokeParticles.SetActive(true);

        }

        // destroy object
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

        // destroy the bomber if it's offscreen
        if (transform.position.x < -12)

        {

            Destroy(gameObject);

        }

    }
    
    void Fire()

    {

        // fire projectile
        Instantiate(projectile, barrel.transform.position, barrel.transform.rotation);

    }

    void DestroyObject()

    {
        
        // add to counters and destroy
        gameManagerScript.BomberScore();
        gameManagerScript.IncreaseKillCount();
        Destroy(gameObject);
        
    }
    
    private void OnCollisionEnter(Collision collision)
    
    {

        // ignore other enemies
        if (collision.gameObject.CompareTag("Enemy"))

        {
            
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());

        }

        // take damage and destroy player projectiles
        if (collision.gameObject.CompareTag("FriendlyProjectile"))

        {

            playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
            playerDamage = playerControllerScript.damage;
            health = health - playerDamage;
            Destroy(collision.gameObject);

        }

    }
    
}