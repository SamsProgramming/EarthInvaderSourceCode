using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour

{

    public int health;
    public float speed;
    public float shotDelay;
    public GameObject barrel;
    public GameObject projectile;

    public int playerDamage;
    private PlayerController playerControllerScript;
    public GameObject smokeParticles;
    public GameObject explosionParticles;

    private bool isDestroyed = false;
    private GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()

    {

        // get game manager script and start firing
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        InvokeRepeating("Fire", 2.0f, shotDelay);

    }
    
    void Update()

    {

        // move left if alive
        if (health > 0)

        {

            transform.Translate(Vector3.up * speed * Time.deltaTime);

        }

        // move left slowly if damaged
        else if (health == 0)

        {

            transform.Translate(Vector3.up * (speed / 2) * Time.deltaTime);
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

        // destroy the tank if it's offscreen
        if (transform.position.x < -12)

        {

            Destroy(gameObject);

        }

    }
    
    void Fire()

    {

        // fire
        Instantiate(projectile, barrel.transform.position, barrel.transform.rotation);

    }
    
    void DestroyObject()

    {
        
        // add counters and destroy
        gameManagerScript.TankScore();
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