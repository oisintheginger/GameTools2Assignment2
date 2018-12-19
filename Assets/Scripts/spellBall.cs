using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellBall : MonoBehaviour {

    Rigidbody rb;
    Vector3 projectForce;
    [SerializeField]
    float projectileSpeed;
    [SerializeField] string target;
    [SerializeField] int damage;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update () {
        projectForce = transform.forward * projectileSpeed ;
        rb.AddForce(projectForce);
        Destroy(gameObject, 3.0f);
	}
    //subtracts damage from player health when projectile hits object with player tag
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag ==target)
        {
            if (target == "Player")
            {
                Destroy(this.gameObject);
                if (other.gameObject.GetComponent<Animator>().GetBool("isBlocking") == false)
                {
                    //other.gameObject.GetComponent<playerMove>().playerHealth -= 1;
                    other.GetComponent<playerMove>().playertakeDamage();
                    other.gameObject.GetComponent<Animator>().SetTrigger("PlayerHurt");
                }

                if (other.gameObject.GetComponent<playerMove>().playerHealth <= 0)
                {
                    other.gameObject.GetComponent<Animator>().SetBool("PlayerDead", true);
                }
            }

            if(target=="Enemy")
            {
                Destroy(this.gameObject);
                other.gameObject.GetComponent<demon>().demonDamage(damage);
                
            }
        }


        else if(other.gameObject.tag=="wall"|| other.gameObject.tag =="Ladder"|| other.gameObject.tag =="Door")
        {
            Destroy(this.gameObject);
        }
    }
}
