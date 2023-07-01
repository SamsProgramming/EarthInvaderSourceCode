using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour

{

    public float speed;

    // Start is called before the first frame update
    void Start()
    
    {

        // start timer until destruction
        StartCoroutine("DespawnProjectile");

    }

    // Update is called once per frame
    void Update()
 
    {

        // move right
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // delete object if its offscreen
        if (transform.position.x < -10)

        {

            Destroy(gameObject);

        }

    }

    IEnumerator DespawnProjectile()

    {

        // wait 15 seconds, destroy object
        yield return new WaitForSeconds(15);
        Destroy(gameObject);

    }

    private void OnCollisionEnter(Collision collision)
    
    {

        // ignore enemies and bosses for collision checking
        if (collision.gameObject.CompareTag("Enemy"))

        {
            
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());

        }
        
        if (collision.gameObject.CompareTag("Boss"))

        {
            
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());

        }
        
    }

}