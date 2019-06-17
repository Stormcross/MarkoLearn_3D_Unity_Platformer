using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject cpOn, cpOff;



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
        if (other.tag=="Player")
        {
            GameManager.instance.SetSpawnPoint(transform.position);
            //Debug.Log("SpawnPointSet!");

            Checkpoint[] allCp = FindObjectsOfType<Checkpoint>(); //spremljeno u memoriji svih checkpointova

            for (int i = 0; i < allCp.Length; i++) //provjeri sve check i postavi samo aktivni zadnji ON
            {
                allCp[i].cpOff.SetActive(true);
                allCp[i].cpOn.SetActive(false);
            }

            cpOff.SetActive(false);
            cpOn.SetActive(true);
        }
    }
}
