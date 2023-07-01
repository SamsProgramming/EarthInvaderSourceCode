using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyProjectile : MonoBehaviour

{

    public float speed;

    // Start is called before the first frame update
    void Start()

    {

        // start time before destroying
        StartCoroutine("DespawnProjectile");

    }

    // Update is called once per frame
    void Update()

    {

        transform.Translate(Vector3.right * speed * Time.deltaTime);
        
        // destroy the project if its offscreen to improve performance and prevent the player from shooting enemies that are offscreen
        if (transform.position.x > 10.5f)

        {

            Destroy(gameObject);

        }
        
    }

    IEnumerator DespawnProjectile()

    {

        // wait 15 seconds, destroy
        yield return new WaitForSeconds(15);
        Destroy(gameObject);

    }

    private void OnCollisionEnter(Collision collision)
    
    {
        
        if (collision.gameObject.CompareTag("Player"))

        {
            
            // ignore player
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());

        }
        
    }
    
}