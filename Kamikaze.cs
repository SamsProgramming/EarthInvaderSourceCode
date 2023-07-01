using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : MonoBehaviour

{
    
    public float speed;
    public int health;
    public bool missedTarget = false;
    
    public GameObject barrel;
    public GameObject bomb;

    public int playerDamage;
    private GameObject player;
    private PlayerController playerControllerScript;
    public GameObject smokeParticles;
    public GameObject explosionParticles;

    private bool isDestroyed = false;
    
    public GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()

    {

        // find player, get game manager, fire explosive once after 2.5 seconds
        player = GameObject.Find("Player");
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        Invoke("LaunchExplosive", 2.5f);

    }

    // Update is called once per frame
    void Update()

    {

        // if alive, move towards players position
        if (health > 0)

        {
            
            if (transform.position.x > player.transform.position.x)

            {
            
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z), (speed * Time.deltaTime));
            
            }

            // if the target is missed, stop following the player and keep moving left
            else
        
            {

                transform.Translate(Vector3.back * speed * Time.deltaTime);

            }
            
        }

        // move left slowly in damaged state
        else if (health == 0)

        {

            transform.Translate(Vector3.back * (speed / 2) * Time.deltaTime);
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

        // destroy the kamikaze if it's offscreen
        if (transform.position.x < -12)

        {

            Destroy(gameObject);

        }

    }

    // launch explosive
    void LaunchExplosive()

    {

        Instantiate(bomb, barrel.transform.position, barrel.transform.rotation);

    }
    
    // add counters and destroy object
    void DestroyObject()

    {
        
        gameManagerScript.KamikazeScore();
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

        // take damage and destroy players projectile
        if (collision.gameObject.CompareTag("FriendlyProjectile"))

        {

            playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
            playerDamage = playerControllerScript.damage;
            health = health - playerDamage;
            Destroy(collision.gameObject);

        }

    }
    
}