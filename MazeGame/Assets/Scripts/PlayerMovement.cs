using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float sideways = 500f;
    public float jump = 100f;

    public float camSensity = 0.25f;
    private Vector3 lastMouse = new Vector3(255, 255, 255);

    public float speed = 20f;
    public Material bulletMat;

    public Camera cam;
    public bool shotDelay = false;
    public bool cantShot = false;   //in maze, cant shot

    public Score sc;

    public GameManager gm;

    private void FixedUpdate()
    {
        //if (Input.GetButton("Fire2"))
        //{
            lastMouse = Input.mousePosition - lastMouse;
            
            lastMouse = new Vector3(0, lastMouse.x * camSensity, 0);

            lastMouse = new Vector3(0, transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;
        //}
        if (Input.GetButton("Fire1"))
        {
 
            if (sc.ammo - sc.usedProjectile > 0 && shotDelay == false && cantShot == false)
            {
                GameObject b = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                b.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                b.transform.localPosition = this.transform.position + new Vector3(0, 1, 1) ;
                b.transform.localRotation = this.transform.rotation * Quaternion.Euler(90, 0, 0);
                b.AddComponent<AirWallBullet>();
                Rigidbody rb = b.AddComponent<Rigidbody>();
                rb.useGravity = false;
                b.GetComponent<Renderer>().material = bulletMat;

                b.GetComponent<CapsuleCollider>().isTrigger = true;
                //b.AddComponent<CapsuleCollider>().enabled = false;

                rb.AddRelativeForce(new Vector3(0, 1, 0) * speed, ForceMode.VelocityChange);
                sc.addProjectileUsed();
                shotDelay = true;
                StartCoroutine(FireDelaySecond());
                gm.checkIfWin();
            }
        }
        if (Input.GetButton("Fire2"))
        {
            lastMouse = this.transform.position;
        }
        if (Input.GetKey("d"))
        {
            rb.AddRelativeForce(sideways * Time.deltaTime, 0, 0, ForceMode.VelocityChange);

        }
        if (Input.GetKey("a"))
        {
            rb.AddRelativeForce(- sideways * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetKey("w"))
        {
            //rb.AddForce(0, 0, sideways * Time.deltaTime, ForceMode.VelocityChange);
            rb.AddRelativeForce(0, 0, sideways * Time.deltaTime, ForceMode.VelocityChange);
        }
        if (Input.GetKey("s"))
        {
            rb.AddRelativeForce(0, 0, -sideways * Time.deltaTime, ForceMode.VelocityChange);
        }
        if (Input.GetKeyDown("space"))
        {
            rb.AddForce(0, jump * Time.deltaTime, 0, ForceMode.VelocityChange);
        }
        if (rb.position.y < -10f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }

    private IEnumerator FireDelaySecond()
    {        
   
        yield return new WaitForSeconds(1);
        shotDelay = false;

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
