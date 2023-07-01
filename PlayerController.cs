using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour

{

    public float speed;
    public int health;
    public int damage;
    public bool readyToFire = true;
    public float rateOfFire;
    public GameObject barrel;
    public GameObject projectile;

    private float horizontalInput;
    private float verticalInput;
    
    private float upperRange = 4.25f;
    private float bottomRange = -4.5f;
    private float leftRange = -8.0f;
    private float rightRange = 8.0f;

    public int score = 0;

    public bool scoreUpgrade = false;
    public bool healthUpgrade = false;
    public bool damageUpgrade = false;
    public bool rateOfFireUpgrade = false;
    public bool speedUpgrade = false;
    
    public List<bool> upgradeList = new List<bool>();

    private int enemyProjectileDamage = 5;
    private int enemyBombDamage = 10;
    
    // mobile movement booleans
    private bool moveLeft = false;
    private bool moveUp = false;
    private bool moveDown = false;
    private bool moveRight = false;
    private bool tryToFire = false;

    void Start()

    {

        // add all possible upgrades to list
        upgradeList.Add(scoreUpgrade);
        upgradeList.Add(healthUpgrade);
        upgradeList.Add(damageUpgrade);
        upgradeList.Add(rateOfFireUpgrade);
        upgradeList.Add(speedUpgrade);
        
    }
    
    // Update is called once per frame
    void Update()
 
    {

        // move player using horizontal and vertical input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // use mobile movement on android
        #if UNITY_ANDROID

            MobileMovement();
        
        // use regular movement on every other platform
        #else

            MovementCheck();

        #endif

        if (health <= 0)

        {

            // reload the scene if player's health is less than zero
            // also create a progression event when the player dies
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }

        if (Input.GetKeyDown(KeyCode.Escape))

        {

            // return to menu using escape, mobile has a unique button appear that does the same
            ReturnToMenu();

        }

    }

    public void FireProjectile()

    {

        // if player is ready to fire
        if (readyToFire == true)

        {

            // create projectile, start delay to shoot again and set ready to false
            Instantiate(projectile, barrel.transform.position, barrel.transform.rotation);
            StartCoroutine("FireDelay");
            readyToFire = false;

        }

    }

    IEnumerator FireDelay()

    {

        // wait for length of time between shots and ready
        yield return new WaitForSeconds(rateOfFire);
        readyToFire = true;

    }
    
    // if player is in bounds, move them around using two axis inputs
    void MovementCheck()
    
    {
    
        if (transform.position.x > leftRange && transform.position.x < rightRange)

        {
            
            transform.Translate(Vector3.forward * (horizontalInput * speed * Time.deltaTime));
            
        }

        if (transform.position.y > bottomRange && transform.position.y < upperRange)

        {

            transform.Translate(Vector3.up * (verticalInput * speed * Time.deltaTime));

        }

        // fire using button one, which is bound in project settings to space
        // side note: this game is playable using a controller, besides clicking buttons it works
        if (Input.GetButton("Fire1"))

        {

            FireProjectile();

        }
        
        if (transform.position.y <= bottomRange || transform.position.y >= upperRange || transform.position.x <= leftRange || transform.position.x >= rightRange)

        {

            MovePlayerBackToBounds();

        }
    
    }

    void MobileMovement()

    {

        if (moveUp && transform.position.y < upperRange)

        {
            
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            
        }

        if (moveDown && transform.position.y > bottomRange)

        {
            
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            
        }

        if (moveLeft && transform.position.x > leftRange)

        {
            
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            
        }

        if (moveRight && transform.position.x < rightRange)

        {
            
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            
        }

        if (tryToFire)

        {
            
            FireProjectile();
            
        }
        
        if (transform.position.y <= bottomRange || transform.position.y >= upperRange || transform.position.x <= leftRange || transform.position.x >= rightRange)

        {

            MovePlayerBackToBounds();

        }
        
    }

    // if player is beyond bounds, move them back in
    // due to not being able to hold buttons and having to click buttons, transform.translate is way faster on mobile, making it buggy but playable
    void MovePlayerBackToBounds()

    {
        
        if (transform.position.y <= bottomRange)

        {
                
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
                
        }
            
        else if (transform.position.y >= upperRange)

        {
                
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.01f, transform.position.z);
                
        }
            
        else if (transform.position.x <= leftRange)

        {
                
            transform.position = new Vector3(transform.position.x + 0.01f, transform.position.y, transform.position.z);
                
        }
            
        else if (transform.position.x >= rightRange)

        {
                
            transform.position = new Vector3(transform.position.x - 0.01f, transform.position.y, transform.position.z);
                
        }
        
    }

    // get upgrade on collision with upgrade
    public void GetUpgrade()

    {

        // choose random number between 0 and length of list
        // note: this will cause bugs as the same upgrade can be chosen more than once
        int upgradeChoice = Random.Range(0, upgradeList.Count);

        // same code for 5 upgrades, set upgrade to true, increase whatever attribute is upgraded (except score, the boolean is enough) and remove from list
        if (upgradeChoice == 0)

        {

            scoreUpgrade = true;
            upgradeList.RemoveAt(upgradeChoice);

        }
        
        else if (upgradeChoice == 1)

        {

            healthUpgrade = true;
            health = health + 100;
            upgradeList.RemoveAt(upgradeChoice);

        }
        
        else if (upgradeChoice == 2)

        {

            damageUpgrade = true;
            damage = damage * 2;
            upgradeList.RemoveAt(upgradeChoice);

        }
        
        else if (upgradeChoice == 3)

        {

            rateOfFireUpgrade = true;
            rateOfFire = rateOfFire / 2;
            upgradeList.RemoveAt(upgradeChoice);

        }
        
        else if (upgradeChoice == 4)

        {

            speedUpgrade = true;
            speed = speed * 1.5f;
            upgradeList.RemoveAt(upgradeChoice);

        }

    }

    // set boolean to true to enable movement by holding button
    public void MoveUp()

    {

        moveUp = true;

    }
    
    public void MoveDown()

    {

        moveDown = true;

    }
    
    public void MoveLeft()

    {

        moveLeft = true;

    }
    
    public void MoveRight()

    {

        moveRight = true;

    }

    public void Fire()

    {

        tryToFire = true;

    }
    
    // opposite of above methods, stop moving once button is released
    public void StopMoveUp()

    {

        moveUp = false;

    }
    
    public void StopMoveDown()

    {

        moveDown = false;

    }
    
    public void StopMoveLeft()

    {

        moveLeft = false;
    }
    
    public void StopMoveRight()

    {

        moveRight = false;

    }

    public void StopFiring()

    {

        tryToFire = false;

    }

    // return to menu as mentioned in update
    public void ReturnToMenu()

    {

        SceneManager.LoadScene("Menu");

    }

    private void OnCollisionEnter(Collision collision)
    
    {

        // if colliding with enemy projectile
        if (collision.gameObject.CompareTag("EnemyProjectile"))

        {

            // see what the name of the projectile is, and take damage accordingly
            // note: not a good way of doing this, however, it works, also smart and regular bombs have to be separate as having it as a if x or y made smart bombs not collide
            if (collision.gameObject.name == "Enemy Projectile Regular(Clone)")

            {

                health = health - enemyProjectileDamage;
                Destroy(collision.gameObject);

            }

            if (collision.gameObject.name == "Enemy Bomb Regular(Clone)")

            {

                health = health - enemyBombDamage;
                Destroy(collision.gameObject);

            }

            if (collision.gameObject.name == "Enemy Bomb Smart(Clone)")

            {
                
                health = health - enemyBombDamage;
                Destroy(collision.gameObject);
                
            }

        }
        
        // if colliding with an enemy
        if (collision.gameObject.CompareTag("Enemy"))

        {

            // ram them! take 20 damage at the cost of instantly killing the enemy
            health = health - 20;
            Destroy(collision.gameObject);

        }
        
        if (collision.gameObject.CompareTag("Boss"))

        {

            // ramming doesnt work on bosses, take 10 damage and get sent back to the center to avoid further collisions
            health = health - 10;
            transform.position = new Vector3(0, 0, transform.position.z);

        }

    }
    
}