using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerMove : MonoBehaviour {

    Rigidbody playerRB;
    Animator anim;

    //HealthBar Stuff
    public float playerHealth;
    [SerializeField] float startHealth, healthRegen;
    [SerializeField] float startMagic, magicRegen;
    private float playerMagic;
    [Header("HealthBar")]
    public Image healthBar;

    [Header("MagicBar")]
    public Image magicBar;


    //Magic Spell Stuff
    [SerializeField] GameObject spellball;
    [SerializeField] Transform spellTransform;

    //Sound stuff
    [SerializeField] AudioClip stepSound, swingSound,jumpSound, landSound;

    AudioSource playerSound;

   
    

    [SerializeField]
    Vector3 JumpForce, forwardForce;
    [SerializeField]
    float strength, maxVert,attackRadius;

    //raycast transform
    [SerializeField]
    Transform raycastHeight;

    //checking for the ground check
    public groundCheck gc;

    //drawing a circle for the range
    

    // Use this for initialization
    void Start () {
        playerMagic = startMagic;
        playerHealth = startHealth;
        //accessing componenets
        playerRB = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        playerSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        magicBar.fillAmount = playerMagic / startMagic;
        healthBar.fillAmount = playerHealth / startHealth;
        if (playerMagic<startMagic)
        {
            playerMagic += magicRegen;
        }
        if(playerHealth<startHealth)
        {
            playerHealth += healthRegen;
        }
        forwardForce = strength * transform.forward;
        //setting the floats in the blend tree to react to player input
        anim.SetFloat("VertSpeed", playerRB.velocity.y);
        anim.SetBool("isGrounded", gc.Grounded);
        float m_turn = Input.GetAxis("Horizontal");
        anim.SetFloat("Turn", m_turn);

        float m_forward = Input.GetAxis("Vertical");
        anim.SetFloat("Forward", m_forward);

        //raycast stuff
        int layerMask = 1 << 9;

        
        RaycastHit hitter;
        if (Physics.Raycast(raycastHeight.position, transform.TransformDirection(Vector3.forward), out hitter, attackRadius, layerMask))
        {
            Debug.DrawRay(raycastHeight.position, transform.TransformDirection(Vector3.forward) * hitter.distance, Color.yellow);
            if(Input.GetMouseButtonDown(0)&&hitter.collider.gameObject.tag=="Enemy")
            {
                Debug.Log("Did Hit");
                hitter.collider.GetComponent<demon>().demonDamage(1);
                
            }
            
        }
        else
        {
            Debug.DrawRay(raycastHeight.position, transform.TransformDirection(Vector3.forward) * attackRadius, Color.white);
            Debug.Log("Did not Hit");
        }


        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("StartMenu");
        }

        //running
        if (Input.GetKey(KeyCode.W)&& anim.GetBool("PlayerDead") == false && anim.GetBool("PlayerClimb") == false)
        {
            if (gc.Grounded == false)
            {

                forwardForce = strength * transform.forward;
                playerRB.AddForce(forwardForce);
            }
            
        }
        if (Input.GetKey(KeyCode.S) && gc.Grounded == false&& anim.GetBool("PlayerDead") == false && anim.GetBool("PlayerClimb")==false)
        {
            forwardForce *= -1;
            playerRB.AddForce(forwardForce);
        }
        //jumping
        if (Input.GetKeyDown(KeyCode.Space) && gc.Grounded == true&& anim.GetBool("PlayerDead") == false)
        {
            StartCoroutine(jumpWait());
        }
        //attack and block
        if (Input.GetMouseButtonDown(0)&& anim.GetBool("PlayerDead") == false)
        {
            anim.SetTrigger("Attack");
        }
        if (Input.GetMouseButton(1)&& anim.GetBool("PlayerDead") == false)
        {
            anim.SetBool("isBlocking", true);
        }
        else if (!Input.GetMouseButton(1)&& anim.GetBool("PlayerDead") == false)
        {
            anim.SetBool("isBlocking", false);
        }

        //rolling
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetTrigger("PlayerRoll");
        }
        //spell cast
        if (Input.GetKeyDown(KeyCode.Q) && anim.GetBool("isBlocking") == false && playerMagic>1)
        {
            castingSpell();
        }
        
    }

    //stepsound
    public void footStepSound()
    {
        StepSound();
    }

    private void StepSound()
    {
        playerSound.volume = 0.076f;
        playerSound.pitch = 1f;
        playerSound.PlayOneShot(stepSound);
    }

    public void deadSceneLoad()
    {
        SceneManager.LoadScene("Level");
    }
    

    //swingsound
    public void swingingSound()
    {
        swingSFX();
    }

    private void swingSFX()
    {
        playerSound.volume = 1;
        playerSound.pitch = 1f;
        playerSound.PlayOneShot(swingSound);
    }

    //jumpsound
    public void jumpingSound()
    {
        jumpSFX();
    }
    private void jumpSFX()
    {
        playerSound.volume = 0.12f;
        playerSound.pitch = 0.9f;
        playerSound.PlayOneShot(jumpSound);
    }

    public void landingSound()
    {
        landSFX();
    }
    private void landSFX()
    {
        playerSound.volume = 0.076f;
        playerSound.pitch = 2.19f;
        playerSound.PlayOneShot(landSound);
    }

    private void castingSpell()
    {
        playerMagic -= 1;
        //magicBar.fillAmount = playerMagic / startMagic;
        anim.SetTrigger("Cast");
    }


    //using a timer rather than an animation event just to show I could do it using an IEnumerator
    IEnumerator jumpWait()
    {
        anim.SetTrigger("JumpUp");
        yield return new WaitForSecondsRealtime(0.1f); //waiting for the animation to be at the correct point before the jumpforce is added
        playerRB.AddForce(JumpForce);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

    }



    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag =="Ladder"&& Input.GetKey(KeyCode.E))
        {
            anim.SetBool("PlayerClimb", true);
        }
       
        if(other.gameObject.name=="LadderTop")
        {
            anim.SetBool("PlayerClimb", false);
            anim.SetTrigger("FinishClimb");
        }

        if(other.gameObject.tag=="Door"&& Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("PlayerOpen");
            if (other.gameObject.GetComponent<Animator>().GetBool("isOpened") == false)
            {
                other.gameObject.GetComponent<Animator>().SetTrigger("Open");
                other.gameObject.GetComponent<Animator>().SetBool("isOpened", true);
            }
            else if(other.gameObject.GetComponent<Animator>().GetBool("isOpen") == true)
            {
                other.gameObject.GetComponent<Animator>().SetTrigger("Close");
                other.gameObject.GetComponent<Animator>().SetBool("isOpened", false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag =="end")
        {
            SceneManager.LoadScene("StartMenu");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag=="Ladder")
        {
            anim.SetBool("PlayerClimb", false);
        }
    }

    public void playertakeDamage()
    {
        playerHealth -= 1;
        //healthBar.fillAmount = playerHealth / startHealth; 
    }

    public void Playerprojectile()
    {
        

            Instantiate(spellball, spellTransform.position, spellTransform.rotation);
            /*audioD.volume = 0.2f;
            audioD.pitch = 1f;
            audioD.PlayOneShot(fireball);
            */
        


    }
}
