using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour

{
    
    public float speed;
    public int damage;
    public bool smart;
    public bool destructible;
    
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    
    {

        // start a timer until projectile gets despawned, also get player 
        StartCoroutine("DespawnProjectile");
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
 
    {

        // if the projectile is smart, it will follow the player
        if (smart)

        {
            
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z), (speed * Time.deltaTime));
            
        }
        
        // else, it will just move left
        else

        {
            
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            
        }

        // if the projectile is on the ground, destroy
        if (transform.position.y <= -4.5)

        {
            
            Destroy(gameObject);
            
        }

    }

    IEnumerator DespawnProjectile()

    {

        // after 15 seconds, destroy object
        yield return new WaitForSeconds(15);
        Destroy(gameObject);

    }
    
    private void OnCollisionEnter(Collision collision)
    
    {

        // ignore enemies
        if (collision.gameObject.CompareTag("Enemy"))

        {
            
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());

        }
        
        // ignore bosses
        if (collision.gameObject.CompareTag("Boss"))

        {
            
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());

        }

        // destroy the bomb only if its destructible, else it does nothing
        if (collision.gameObject.CompareTag("FriendlyProjectile"))

        {

            if (destructible)

            {

                Destroy(gameObject);
                Destroy(collision.gameObject);
                
            }

        }

    }
    
}