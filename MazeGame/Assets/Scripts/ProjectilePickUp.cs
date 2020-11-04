using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePickUp : MonoBehaviour
{
    /* no longer in use
     * replaced by another method in PlayerCollisio
     */
    public Score sc;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            Debug.Log("triggered！");
            Physics.IgnoreCollision(this.GetComponent<Collider>(), other.GetComponent<Collider>());
            Destroy(this.gameObject);
            sc.addAmmo();
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
