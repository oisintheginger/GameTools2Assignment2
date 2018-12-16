using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellBall : MonoBehaviour {

    Rigidbody rb;
    Vector3 projectForce;
    [SerializeField]
    float projectileSpeed;

    
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
        if(other.gameObject.tag =="Player")
        {
            Destroy(gameObject);
            if (other.gameObject.GetComponent<Animator>().GetBool("isBlocking") == false)
            {
                other.gameObject.GetComponent<playerMove>().playerHealth -= 1;
            }

            if (other.gameObject.GetComponent<playerMove>().playerHealth == 0 )
            {
                other.gameObject.GetComponent<Animator>().SetBool("PlayerDead", true);
            }
        }
    }
}
