using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour

{

    public float speed;
    public float shotDelay;
    public int health;
    
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

        // get player, game manager script and start firing after 2 second delay so that it doesnt fire off screen
        player = GameObject.Find("Player");
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        InvokeRepeating("Fire", 2.0f, shotDelay);

    }

    // Update is called once per frame
    void Update()

    {

        // if alive, chase player on y axis after moving into appropriate position
        if (health > 0)

        {

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, player.transform.position.y, transform.position.z), (speed * Time.deltaTime));

            if (transform.position.x != 8)

            {

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(8, transform.position.y, transform.position.z), (speed * Time.deltaTime));

            }

        }

        // enter damaged state on 0 health
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
        
        // destroy the fighter if it's offscreen
        if (transform.position.x < -12)

        {

            Destroy(gameObject);

        }

    }

    void Fire()

    {


        // fire from barrel
        Instantiate(projectile, barrel.transform.position, barrel.transform.rotation);

    }
    
    void DestroyObject()

    {
        
        // add counters and destroy
        gameManagerScript.FighterScore();
        gameManagerScript.IncreaseKillCount();
        Destroy(gameObject);
        
    }
    
    private void OnCollisionEnter(Collision collision)
    
    {

        // ignore enemies
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