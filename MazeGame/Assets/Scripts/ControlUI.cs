using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour
{

    public GameManager gm;
    public Text text;


    private void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.name == "Player")
        {
            //gm.ControlInstructionUI();
            text.text = "If You're Stuck at the Entrance of the Maze, Try to Move Your Mouse to Change Movement Directions!";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
