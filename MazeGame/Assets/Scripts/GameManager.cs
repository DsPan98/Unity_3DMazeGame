using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    bool gameEnded = false; 
    //end game indicator

    public float restartDelay = 1f;
    public GameObject fallInCanyonUI;
    public GameObject projectileUI;
    public GameObject controlInstructionUI;
    public GameObject restartUI;
    public GameObject inMazeUI;
    //UI for finishing the game, either win or lose


    public Material[] randomMat;
    public GameObject[] colorGround;
    //Random setting for Groundspace

    public int groundX = 10;
    public int groundZ = 25;
    public int cubeLength = 1;
    public List<float> storeX = new List<float>();
    public List<float> storeZ = new List<float>();
    public float startX;
    public float startZ;
    public float endX;
    public float endZ;
    public float currX;
    public float currZ;

    public bool stopper = true;
    public int prevDecision = 0;

    public Material pathMat;
    public Material pathMat2;
    public int projectileAmmo = 8;

    public UpliftTrigger t1;
    public DownTrigger t2;
    public GameObject wUI;
    public Text tl;
    public Text tw;
    public GameObject lUI;
    public GameObject canvas;

    public bool winGame = false;
    public Score sc;

    public MazeGenerate mg;
    public Material mat3;

    public bool pathCreate = true;


   
    //parameters used for implementing randomized path



    //+-----------EndGame()--------+
    /*
     *EndGame() method ends the game and restarts the game from the begining
     *It checks for two gameend indicators
     *1. player fall off canyon
     *2. manually pressing 'esc' key
     */
    public void EndGame()
    {//endgame method
        if(gameEnded == false)
        {
            Debug.Log("GameOver");
            Invoke("Restart", restartDelay);
        }
        else
        {//gameEnd == true

        }
    }
    
    /* 
     * Reload the game, using a load scene function, loading back to the current scene
     */
    void Restart()
    {//restarts the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Start()
    {
        sc.setAmmo(0);
        //+--------------------+
        /*Start the game with a randomly selected color
         * assigned to the Solid ground
         *Function-wise it does nothing, but just makes every game a tiny bit different
         */
        foreach (GameObject ground in colorGround)
        {
            ground.GetComponent<Renderer>().material = randomMat[Random.Range(0, randomMat.Length)];
        }

        //+--------------------+
        /* for random path
         * The path is randomly generated, from the starting point, far away
         * from the maze, to the end of the canyon.
         * The road is guarenteed to be UNICURSAL, and no circles would be present
         */
        startX = 0;
        startZ = -(groundZ - cubeLength) / 2;
        endX = 0;
        endZ = (groundZ - cubeLength) / 2;
        currX = startX;
        currZ = startZ;

        storeX.Add(startX);
        storeZ.Add(startZ);
        /*
         * The random path generator
         * start off with an infinite loop
         * 1) For every iteration, check whether the previous tile has reached
         * the desinated location; if it did, then break the loop; else continue
         * 2) Randomly assign a direction, left, or right, or forward
         * 3) Check if the current tile has any restrictions performing a movement
         * towards the assigned direction; change the tile transform.position to the 
         * corresponding location
         * 4) memorize and update the new current
         * 5) continue looping
         */
        while (stopper)
        {
            if ((currX <= (endX + 0.5) && currX >= (endX - 0.5)) && ((currZ <= (endZ + 0.5) && currZ >= (endZ - 0.5))))
            { // current path tile has reached the destinated location, the path is generated
                stopper = false;
                //Debug.Log(storeX.Count);
            }
            else
            { // else the path is still generating
              // starting from the first point, 3 directions: forward, left, and right could be selected; no going backwards
                int rand = Random.Range(1, 6); // 1 2 3 4 5
                if (rand == 1 || rand == 4)
                {//go left
                    if (prevDecision == 3 || prevDecision == 5 || currX <= -(groundX) / 2)
                    {// out of boundry, go forward
                        if (currZ > groundZ / 2 - 0.5)
                        {
                            stopper = false;
                        }
                        else
                        { // else go forward
                            currZ += 1;
                            storeX.Add(currX);
                            storeZ.Add(currZ);
                        }
                    }
                    else
                    {//go left
                        currX -= 1;
                        storeX.Add(currX);
                        storeZ.Add(currZ);
                    }

                }
                if (rand == 2)
                {//go mid
                    if (currZ > groundZ / 2 - 0.5)
                    {
                        stopper = false;
                    }
                    else
                    {
                        currZ += 1;
                        storeX.Add(currX);
                        storeZ.Add(currZ);
                    }
                }
                if (rand == 3 || rand == 5)
                {// go right
                    if (prevDecision == 1 || prevDecision == 4 || currX >= (groundX / 2))
                    {//go straight this condition
                        if (currZ > groundZ / 2 - 0.5)
                        {
                            stopper = false;
                        }
                        else
                        {
                            currZ += 1;
                            storeX.Add(currX);
                            storeZ.Add(currZ);
                        }
                    }
                    else
                    {//go right
                        currX += 1;
                        storeX.Add(currX);
                        storeZ.Add(currZ);
                    }
                }
                prevDecision = rand;
            }
        }
        /*
         * For every single current tile location collected in the previous section
         * add a tile to that location
         * A color would be randomly selected for the initial tile, and every other tile would have the same color (so a pattern)
         * The tiles are set to have no collision with the player, so player wouldn't tripped by the tiles and fall down (which proven to be a huge problem during develop phase)
         * The tiles are also set to have constrained x and z positions and rotations, so they would flipped and fall off the edges
         */
        List<int> arr = new List<int>();
        for (int i = 1; i < storeX.Count; i++)
        { // stored point would be (storeX[i], storeZ[i]), corresponding vector3 (storeX[i] - groundX/2, y, storeZ[i] - groundZ/2);
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localScale = new Vector3(1, 0.0125f, 1);
            cube.transform.position = new Vector3(storeX[i], 0.50625f, storeZ[i]);

            //tag added to ignore collision with player
            
            //cube.tag = "IgnoreCollision";
            Rigidbody rb = cube.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            
            BoxCollider bc = cube.AddComponent<BoxCollider>();
            bc.material.dynamicFriction = 0;
            bc.material.staticFriction = 0;
            Renderer rend = cube.GetComponent<Renderer>();
            //rend.material = (i % 2 == 1) ? pathMat : pathMat2;
            rend.material = pathMat;

            arr.Add(i);
        }
        List<int> randLocationIndex = new List<int>();
        /* After path tiles being added to game
         * the projectiles would spawn on top of it
         * they do not have ridigbodies and therefore would not make any phyiscal behavior
         * ALL Projectiles are Guarenteed to spawn on top of a path tile, and their spawn location would be random
         */
        for (int i = 0; i < projectileAmmo; i++)
        {
            int k = Random.Range(0, arr.Count);
            randLocationIndex.Add(k);
            arr.RemoveAt(k);
        }

        for (int i = 0; i < randLocationIndex.Count; i++)
        {
            GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            projectile.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            projectile.transform.position = new Vector3(storeX[randLocationIndex[i]], 1, storeZ[randLocationIndex[i]]);
            projectile.layer = 8;
            projectile.transform.gameObject.tag = "Projectile";
            //CapsuleCollider cc = projectile.AddComponent<CapsuleCollider>();
            //cc.isTrigger = true;
            projectile.GetComponent<CapsuleCollider>().enabled = false;
            //ProjectilePickUp pp = projectile.AddComponent<ProjectilePickUp>();
            //ProjectilePickUp no longer in use


        }
    }

    /*
     * Not much happening for Update() function
     * most of the work is done in Start() method
     * The rest would bedetected by the player-related functions
     * The only thing checked in update is 'ESC' restart game
     */
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscRestart();
        }   
        checkIfLose();
        if (t2.isTriggered == true)
        {
            checkIfWin();
        }
    }

    //+------------UI Triggering Functions-----------+
    /*
     * The methods below are used for calling out different UI or functions
     * when the trigger detects a collision
     * FallInCanyon() is triggered when the player gameobject collides with
     * the canyon trigger detection gameobject and gives out the 'you lose xxxx' UI
     * EscRestart() detects esc enter, and restarts the game
     * ProjectileUI() is introduced at the start of the game, and reintroduced
     * everytime the game restarts
     * ControlInstructionUI() is introduced at the start of th egame, and reintroduced
     * everytime the game restarts; at midpoint of the ground path, there's a trigger
     * detection gameobject; when detected collision with the player, the control instruction
     * UI scene would rapidly fade out (considering the player is half way through and 
     * they certainly know how to operate)
     */
    public void FallInCanyon()
    {
        fallInCanyonUI.SetActive(true);
        projectileUI.SetActive(false);
    }
    public void EscRestart()
    {
        restartUI.SetActive(true);
        projectileUI.SetActive(false);
        FindObjectOfType<GameManager>().EndGame();
    }
    public void ProjectileUI()
    {
        projectileUI.SetActive(true);
    }
    public void ControlInstructionUI()
    {
        controlInstructionUI.SetActive(false);
    }
    public void InMazeUIEnable()
    {
        inMazeUI.SetActive(true);
    }
    public void InMazeUIDisable()
    {
        inMazeUI.SetActive(false);
    }

    public void checkIfWin()
    {

        /* The game has only one winning condition and multiple factors to check
         * 1) the player has to pass the maze (using a trigger to check)
         * 2) the player has to shot at least one projectile and hit a tile that could lead to a path
         */
        //winGame = true;
        List<GameObject> a = new List<GameObject>();
        a = AirWallBullet.destroyedTiles;
        if (a != null)
        {
            for (int i = 0; i < a.Count; i++)
            {
                for (int j = 0; j < mg.onlyPath.Count; j++)
                {
                    float x = (mg.onlyPath[j].posIndex % 5) * 3 + 0.5f;
                    float y = (mg.onlyPath[j].posIndex / 5) * 3 + 0.5f;
                    float tx = a[i].transform.position.x + 7;
                    float ty = a[i].transform.position.z + 19;
                    if (((x + 0.5f) >= tx) && ((x - 0.5f) <= tx))
                    {
                        if (((y + 0.5f) >= ty) && ((y - 0.5f) <= ty))
                        {
                            WinGame();
                        }
                    }
                }
            }
        }       
    }
    public void checkIfLose()
    {
        /* The game could have multiple losing conditions
         * 1) the player has not entered the maze, yet all available ammo fired
         * 2) the player has entered the maze, and no ammo is collected
         * 3) the player passed the maze, all projectiles are used and game not over
         */
        if (t1.isTriggered == false && sc.usedProjectile == 8)
        { // player has not yet entered the maze, but all available projectiles are used
            LoseGame("You Ran Out Of Ammo Before Even Entering the Maze!");
        }
        if (t1.isTriggered == true && (sc.ammo - sc.usedProjectile) == 0)
        {
            //in this case no projectile is collected
            LoseGame("You Ran Out Of Ammo Before Even Finishing the Maze!");
        }
        if(t2.isTriggered == true && (sc.ammo - sc.usedProjectile) == 0)
        {
            Invoke("checkIfWin", 2.0f);
            if(t2.isTriggered == true && (sc.ammo - sc.usedProjectile) == 0)
            {
                //for two seconds, if gamemanager still doesnt detect a win, then the player lose
                //(consider possibility the player 
                LoseGame("You Missed Your Shot!");
            }

        }  
    }
    public void LoseGame(string str)
    {   
      
      
        lUI.SetActive(true);
        tl.text = str + "\nand its GameOver\nYou Lose! \nClick ESC to Restart!";
        canvas.SetActive(false);
        Debug.Log("The correct path for this randomized maze was:\n");
        for(int k = 0; k < mg.onlyPath.Count; k++)
        {
            int x = mg.onlyPath[k].posIndex % 5;
            int y = mg.onlyPath[k].posIndex / 5;
            Debug.Log("->(" + x + ", " + y + " ) ");

        }


    }
    public void WinGame()
    {
        wUI.SetActive(true);
        canvas.SetActive(false);

        Debug.Log("The correct path for this randomized maze was:\n");
        for (int k = 0; k < mg.onlyPath.Count; k++)
        {
            int x = mg.onlyPath[k].posIndex % 5;
            int y = mg.onlyPath[k].posIndex / 5;
            Debug.Log("->(" + x + ", " + y + " ) ");

        }

    }
}
