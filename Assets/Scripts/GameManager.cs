using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //static je ista varijabla za sve varijable u game managerima kroz cijelu igru

    private Vector3 respawpPosition; //varijabla za spremanje starne pozicije


    private void Awake() //awake je prva stvar koja se desi, i ona se desava prijke void start
    {
        instance = this;//postavljamo instanance na bilo koji this game object
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        respawpPosition = PlayerController.instance.transform.position; //spremljena pozicija pocetnog polozaja

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Respawn()
    {
        StartCoroutine(RespawnCo()); //radi zasebno kao korutina za sebe
    }

    public IEnumerator RespawnCo() //desavalo se bude zasebno uz ostale funkcije dok se pokrene, ne ceka proceduralni postupak
    {
        // Debug.Log("I am respawning"); //provjera ako se bude aktivirao
        PlayerController.instance.gameObject.SetActive(false);

        CameraController.instance.theCMBrain.enabled = false; //to ugasi posebno slijedenje playera i kamera postaje normalna fiksna

        UIManager.instance.fadeToBlack = true; //nakon respawna nam se pojavi crni ekran

        yield return new WaitForSeconds(2f);

        UIManager.instance.fadeFromBlack = true;

        PlayerController.instance.transform.position = respawpPosition;

        CameraController.instance.theCMBrain.enabled = true;


        PlayerController.instance.gameObject.SetActive(true);
    }

}
