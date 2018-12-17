using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            
            other.gameObject.GetComponent<Animator>().SetBool("PlayerClimb", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            
            other.gameObject.GetComponent<Animator>().SetBool("PlayerClimb", false);
        }
    }
}
