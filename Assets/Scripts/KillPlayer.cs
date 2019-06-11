using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //provjerimo ako nam player ulazi u area, tags "player" ime colidera
        //tag se definira  u unity inspektoru Tag
        if (other.tag=="Player")
        {
            //Debug.Log("Entered kill zone"); //saljemo si u unity poruku
            GameManager.instance.Respawn(); //pozivamo instancu u game manageru i povezana je na sve game managere
        }
    }
}
