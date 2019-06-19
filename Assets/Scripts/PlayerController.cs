using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    public float jumpForce;
    public float gravityScale = 5f;

    private Vector3 moveDirection;

    public CharacterController charController;

    //za kameru
    private Camera theCam;

    //
    public GameObject playerModel;

    //brzina slerp rotacije
    public float rotateSpeed;

    //referenca na animaciju
    public Animator anim;

    public bool isKnocking;
    public float knockBackLength = .5f;
    private float knockBackCounter;
    public Vector2 knockBackPower;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //zelimo setapirati kameru
        theCam = Camera.main; //postavimo je kao glavnu kameru
    }

    // Update is called once per frame
    void Update()
    {
        if (!isKnocking)
        {

            //lokalna varijabla za odrzavanje move directiona
            float yStore = moveDirection.y;

            //moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")); //koristimo unity inpuyt system za kretanje Edit -Project Settings
            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal")); //u kojem smjeru trenutno player gleda
                                                                                                                                     //mnozimo s vertikalom i horizontalom jer zelimo imati naprijed i iza i lijevo i desno zasebno
                                                                                                                                     //normalize je funkcija koja prema krivulji normalizira vrijednost umjesto da vektorski zbraja
            moveDirection.Normalize();
            moveDirection *= moveSpeed; //odmah ubacimo move speed da se nadoveze na sve kretnje
            moveDirection.y = yStore;

            //postavimo uvijet ako je player na podu onda moze jump

            if (charController.isGrounded)
            {
                moveDirection.y = 0f;

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpForce; //provjera ako je pritisnuti jump (space) button u nekon trenutku
                }
            }

            //ubacimo gravitaciju na jump dodat ce -9.81 gravitacije na y
            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

            //rade zelimo koristiti charController
            //transform.position = transform.position + (moveDirection*Time.deltaTime*moveSpeed); //time.delta zelimo da se isto krece bez obrzira na frame rate i pomnozeno s move speed s kojom mozemo kontrolirati

            charController.Move(moveDirection * Time.deltaTime);

            //kamera postavimo preko eulera nacin 1
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) //ako ima inputa na horizontalu ili vertikali poravnaj kameru
                                                                                          //ako nema inputa onda moremo na mjestu rotirati kameru
                                                                                          //ovdje provjeravamo ako ima inputa onda pratimo playera
            {
                transform.rotation = Quaternion.Euler(0f, theCam.transform.rotation.eulerAngles.y, 0f); //euler postavlja u vector3 jer je quatternion bog pomagaj broj - Y axis je za okret
                                                                                                        //stvaramo novu rotaciju da okrecemo playera
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
                //ne mjenjamo glavni player, mjenjamo samo model da se okrene
                //playerModel.transform.rotation = newRotation; //snapa na kut,zelimo smooth rotacije
                //slerp je rotacijska verzija lerp, to je svojstvo da se stvori smooth pokret!!!
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            }

        }

        if (isKnocking)
        {
            knockBackCounter -= Time.deltaTime;

            float yStore = moveDirection.y;
            moveDirection =playerModel.transform.forward * -knockBackPower.x ;
            moveDirection.y = yStore;


            if (charController.isGrounded)
            {
                moveDirection.y = 0f;
            }

            //ubacimo gravitaciju na jump dodat ce -9.81 gravitacije na y
            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;


            charController.Move(moveDirection*Time.deltaTime);

            if (knockBackCounter<=0)
            {
                isKnocking = false;
            }
        }

        //dodamo za animaciju, mozemo samo jednu vrijednost samo zato ih zbrajamo, stavljamo kao absolutnu da imamo uvijek pozitivnu vrijednost
        anim.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));

        //postavljanje uvijeta ako je player grounded, na nekoj povrsini
        anim.SetBool("Grounded", charController.isGrounded); //setamo value prema animator

    }


    public void KnockBack()
    {
        isKnocking = true;
        knockBackCounter = knockBackLength;
        Debug.Log("Knocked Back!");
        moveDirection.y = knockBackPower.y;
        charController.Move(moveDirection * Time.deltaTime);
    }
}
