using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour

{

    public float speed;
    public int health;
    public bool spawnCooldown = true;

    public int playerDamage;
    private PlayerController playerControllerScript;
    public GameObject explosionParticles;

    public GameObject kamikaze;
    public GameObject bomber;
    public GameObject ufo;

    public GameObject spawner1;
    public GameObject spawner2;
    public GameObject spawner3;
    public GameObject spawner4;
    public GameObject spawner5;

    private bool isDestroyed = false;
    private GameManager gameManagerScript;

    void Start()

    {

        // get game manager and start the spawn delay coroutine, which will give time for the player to get ready and for the boss to get in position
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        StartCoroutine("SpawnDelay");

    }

    // Update is called once per frame
    void Update()

    {

        // move into position if not at x = 6.5
        if (transform.position.x != 6.5)

        {

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(8, transform.position.y, transform.position.z), (speed * Time.deltaTime));

        }

        // if the cooldown is off
        if (spawnCooldown == false)

        {
            
            // stage 1 - between 4500 and 3000 health, spawn 5 kamikazes and set rotations of spawners to make sure enemies come out correctly
            if (health > 3000)

            {

                spawner1.transform.rotation = Quaternion.Euler(0, 90, 0);
                spawner2.transform.rotation = Quaternion.Euler(0, 90, 0);
                spawner3.transform.rotation = Quaternion.Euler(0, 90, 0);
                spawner4.transform.rotation = Quaternion.Euler(0, 90, 0);
                spawner5.transform.rotation = Quaternion.Euler(0, 90, 0);
                Stage1Spawn();

            }
            
            // stage 2 - 3000 to 1500 health, spawn 2 fighters and 3 kamikazes, again, rotate spawners to ensure proper rotation of enemies
            else if (health > 1500)

            {

                spawner1.transform.rotation = Quaternion.Euler(0, 90, 0);
                spawner2.transform.rotation = Quaternion.Euler(0, 270, 0);
                spawner3.transform.rotation = Quaternion.Euler(0, 90, 0);
                spawner4.transform.rotation = Quaternion.Euler(0, 270, 0);
                spawner5.transform.rotation = Quaternion.Euler(0, 90, 0);
                Stage2Spawn();

            }

            // stage 3 - 1500 to 0 health, spawn 1 ufo and 4 bombers, rotate spawners
            else
            
            {
                
                
                spawner1.transform.rotation = Quaternion.Euler(0, 0, 0);
                spawner2.transform.rotation = Quaternion.Euler(0, 270, 0);
                spawner3.transform.rotation = Quaternion.Euler(0, 270, 0);
                spawner4.transform.rotation = Quaternion.Euler(0, 270, 0);
                spawner5.transform.rotation = Quaternion.Euler(0, 270, 0);
                Stage3Spawn();
                
            }
            
        }

        // if the boss dies, destroy object
        if (health <= 0)

        {

            if (isDestroyed == false)

            {

                isDestroyed = true;
                Destroy(gameObject.GetComponent<BoxCollider>());
                explosionParticles.SetActive(true);
                Invoke("DestroyObject", 1.0f);

            }

        }

    }

    void Stage1Spawn()

    {

        // spawn 5 kamikazes and start delay
        if (spawnCooldown == false)

        {

            Instantiate(kamikaze, spawner1.transform.position, spawner1.transform.rotation);
            Instantiate(kamikaze, spawner2.transform.position, spawner2.transform.rotation);
            Instantiate(kamikaze, spawner3.transform.position, spawner3.transform.rotation);
            Instantiate(kamikaze, spawner4.transform.position, spawner4.transform.rotation);
            Instantiate(kamikaze, spawner5.transform.position, spawner5.transform.rotation);
            StartCoroutine("SpawnDelay");
            spawnCooldown = true;

        }
        
    }
    
    void Stage2Spawn()

    {
        
        // spawn 2 bombers and 3 kamikazes, start delay
        Instantiate(kamikaze, spawner1.transform.position, spawner1.transform.rotation);
        Instantiate(bomber, spawner2.transform.position, spawner2.transform.rotation);
        Instantiate(kamikaze, spawner3.transform.position, spawner3.transform.rotation);
        Instantiate(bomber, spawner4.transform.position, spawner4.transform.rotation);
        Instantiate(kamikaze, spawner5.transform.position, spawner5.transform.rotation);
        StartCoroutine("SpawnDelay");
        spawnCooldown = true;
        
    }
    
    void Stage3Spawn()

    {
        
        // spawn ufo and 4 bombers, start delay
        Instantiate(ufo, spawner1.transform.position, spawner1.transform.rotation);
        Instantiate(bomber, spawner2.transform.position, spawner2.transform.rotation);
        Instantiate(bomber, spawner3.transform.position, spawner3.transform.rotation);
        Instantiate(bomber, spawner4.transform.position, spawner4.transform.rotation);
        Instantiate(bomber, spawner5.transform.position, spawner5.transform.rotation);
        StartCoroutine("SpawnDelay");
        spawnCooldown = true;
        
    }

    IEnumerator SpawnDelay()

    {
        
        // wait for 10 seconds before allowing more enemies to spawn
        yield return new WaitForSeconds(10.0f);
        spawnCooldown = false;

    }

    void DestroyObject()

    {

        // increase counters
        gameManagerScript.FinalBossScore();
        gameManagerScript.IncreaseKillCount();

        // get achievement 3 for killing final boss
        if (PlayerPrefs.GetInt("Achievement3") != 1)

        {

            PlayerPrefs.SetInt("Achievement3", 1);

        }
        
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