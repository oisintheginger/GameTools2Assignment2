using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restart : MonoBehaviour {

    [SerializeField] string SceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            restartFunction();
        }
    }

    public void restartFunction()
    {
        SceneManager.LoadScene(SceneToLoad);
    }
}
