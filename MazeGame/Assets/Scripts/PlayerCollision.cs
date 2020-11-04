using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;
    public GameObject gm;
    public Score sc;

   
    void OnCollisionEnter(Collision collisionInfo)
    {
        //two types of collision object
        // 1. colliding with impassible terrain: the mountains and walls
        // 2. colliding with invisible wall in the canyon, game over
        // 3. colliding with walls in the maze, nothing happens
       if(collisionInfo.collider.tag == "Canyon")
        {
            movement.enabled = false;
            FindObjectOfType<GameManager>().EndGame();
        }
       /*
       if(collisionInfo.collider.tag == "IgnoreCollision")
        {
            Physics.IgnoreCollision(collisionInfo.collider, player.GetComponent<Collider>());
        }
       */
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] allProjectile = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject t in allProjectile)
        {
            //if (t.transform.position.x == this.transform.position.x && t.transform.position.z == this.transform.position.z)
            if (((this.transform.position.x + 0.5f) >= t.transform.position.x) && ((this.transform.position.x - 0.5f) <= t.transform.position.x))
            {
                if (((this.transform.position.z + 0.5f) >= t.transform.position.z) && ((this.transform.position.z - 0.5f) <= t.transform.position.z))
                {
                    Destroy(t);
                    sc.addAmmo();
                }

            }
        }
    }
}