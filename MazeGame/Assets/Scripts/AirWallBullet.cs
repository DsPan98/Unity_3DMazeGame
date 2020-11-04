using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirWallBullet : MonoBehaviour
{
    public UpliftTrigger trigger;
    public static List<GameObject> destroyedTiles = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
           Destroy(gameObject);
        }
        else
        {
            this.GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(DelaySecond());
        }
    }


    private IEnumerator DelaySecond()
    {

        yield return new WaitForSeconds(0.1f);
        this.GetComponent<CapsuleCollider>().enabled = true;

    }
    // Start is called before the first frame update
    void Start()
    {
        UpliftTrigger trigger = GameObject.Find("UpliftTrigger").GetComponent<UpliftTrigger>();
        this.trigger = trigger;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] allTiles = GameObject.FindGameObjectsWithTag("Tile");
        List<GameObject> onlyTiles = new List<GameObject>();
        for(int i = 0; i < allTiles.Length; i++)
        {   
            if(allTiles[i].name != "Wall(Clone)")
            {
                onlyTiles.Add(allTiles[i]);
            }
        }
  
        foreach(GameObject t in onlyTiles)
        {
            if(((this.transform.position.x + 2.5f) >= t.transform.position.x)&& ((this.transform.position.x - 2.5f) <= t.transform.position.x))
            {
                if (((this.transform.position.z + 2.5f) >= t.transform.position.z) && ((this.transform.position.z - 2.5f) <= t.transform.position.z))
                {
                    if(trigger.isTriggered == true)
                    {
                        Destroy(this);
                        //t.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 20, 0), ForceMode.VelocityChange);
                        //Invoke("Destroy(t)", 2f);
                        destroyedTiles.Add(t);
                        Destroy(t);
                    }
                }
            }
        }

    }
}
