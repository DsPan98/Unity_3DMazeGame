using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpliftTrigger : MonoBehaviour
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
        if(other.gameObject.name == "Player")
        {
            other.transform.position = other.transform.position + new Vector3(0, 2, 0.75f);
            this.transform.GetComponent<BoxCollider>().isTrigger = false;
            gm.ControlInstructionUI();
            gm.InMazeUIEnable();
            pm.cantShot = true;
            isTriggered = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
