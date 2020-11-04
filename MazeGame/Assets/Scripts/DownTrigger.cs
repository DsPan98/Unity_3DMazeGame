using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTrigger : MonoBehaviour
{
    public GameManager gm;
    public PlayerMovement pm;
    public bool isTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            other.transform.position = other.transform.position + new Vector3(0, 0, 1);
            this.transform.GetComponent<BoxCollider>().isTrigger = false;
            gm.InMazeUIDisable();
            pm.cantShot = false;
            isTriggered = true;
            //after leaving the maze, the system can start to check if player has won the game or not
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
