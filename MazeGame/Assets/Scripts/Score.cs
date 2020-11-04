using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public Transform player;
    public Text scoreText;  //UI currently have x projectile ammo
    public Text scoreText2;
    public Text scoreText3;
    public int ammo = 0;
    public int usedProjectile = 0;

    public bool allProjectileCollected = false;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    public void addProjectileUsed()
    {
        usedProjectile = usedProjectile + 1;
    }
    public void addAmmo()
    {
        ammo = ammo + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (ammo == 8)
        {
            allProjectileCollected = true;
        }
 
        scoreText.text = "You currently have: "+ (ammo - usedProjectile).ToString("0") + " projectiles available!";
        if (!allProjectileCollected)
        {
            scoreText2.text = "You successfully picked up " + ammo.ToString("0") + "/8 projectiles!";
        }
        else
        {
            Destroy(scoreText2);
            scoreText3.text = "All Projectiles are Collected!";
            Destroy(scoreText3, 6.0f);
        }
        
        

    }

    public int getUsedProjectile()
    {
        return usedProjectile;
    }
    public int getAmmo()
    {
        return ammo;
    }
    public void setAmmo(int i)
    {
        ammo = i;
    }
}
