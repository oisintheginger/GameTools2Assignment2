using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class demon : MonoBehaviour {
    //demon health bar stuff
    [Header("Demon Healthbar")]
    public Image DemonHealthBar;

    //Nav Mesh stuff
   

    public Transform player;
    public GameObject projectileObj;
    private float DemonHealth;
    [SerializeField] int demonStartHealth;
    

    [SerializeField]
    private float lookRadius, moveSpeed, attackSpellRadius, moveRadius;
    [SerializeField]
    Vector3 thisPos, playerPos;
    [SerializeField]
    private Transform projectileTransform;

    Animator animD;
    Rigidbody demonRB;

    //sound stufff
    AudioSource audioD;
    [SerializeField] AudioClip fireball;


    private void Start()
    {
        DemonHealth = demonStartHealth;


        animD = GetComponent<Animator>();
        demonRB = GetComponent<Rigidbody>();
        audioD = GetComponent<AudioSource>();

        
    }





    // Update is called once per frame
    void Update () {
        thisPos = transform.position;
        playerPos = player.transform.position;

        //movement and distance detection floats
        float sec = moveSpeed * Time.deltaTime;
        float distXZ = Vector3.Distance(playerPos, thisPos);
        float distY = Mathf.Abs(player.position.y - transform.position.y);
        
        
        if (distXZ < moveRadius&& distXZ>attackSpellRadius && distY < 0.5f && animD.GetBool("DemonDead") == false)
        {
            animD.SetBool("DemonRunning", true);
            transform.position = Vector3.MoveTowards(thisPos, player.transform.position,sec);
        }
        
        else if(distXZ>moveRadius|| distY > 0.2f&& animD.GetBool("DemonDead") == false)
        {
            if (distXZ < attackSpellRadius)
            {

                animD.SetTrigger("DemonAttack");
                animD.SetBool("DemonRunning", false);
            }
            else if(distXZ > attackSpellRadius&& animD.GetBool("DemonDead") == false)
            {
                animD.SetBool("isAttacking", false);
            }
            animD.SetBool("DemonRunning", false);
        }
        if (distXZ < lookRadius&& distY < 0.5f&& animD.GetBool("DemonDead") == false)
        {
            transform.LookAt(player);
        }

        
    }



    //public void to be accessed by the animation event to instantiate projectile prefab
    public void projectile()
    {
        if (animD.GetBool("DemonDead")==false)
        {
            
            Instantiate(projectileObj, projectileTransform.position, projectileTransform.rotation);
            audioD.volume = 0.2f;
            audioD.pitch = 1f;
            audioD.PlayOneShot(fireball);
        }
        
        
    }



    public void demonDamage(int damagetaken)
    {
        DemonHealth -= damagetaken;
        DemonHealthBar.fillAmount = DemonHealth / demonStartHealth;
        if (DemonHealth == 0)
        {
            animD.SetBool("DemonDead", true);
        }
       
    }






    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, moveRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackSpellRadius);
    }
}
