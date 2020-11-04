using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnsuccessFall : MonoBehaviour
{

    public GameManager gm;

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "Player")
        {
            gm.FallInCanyon();
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
