using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenProjectileUI : MonoBehaviour
{
    public GameManager gm;
    private void OnTriggerEnter()
    {
        gm.ProjectileUI();
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
