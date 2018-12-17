using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour {

    Vector2 mouseLook;
    Vector2 smoothV;
    //public float sensitivity = 5.0f;
    //public float smoothing = 2.0f;
    [SerializeField] float sensitivity, smoothing, verticalAngleLower, verticalAngleHigher;
    private GameObject character;
	// Use this for initialization
	void Start () {
        character = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;
        mouseLook.y = Mathf.Clamp(mouseLook.y, verticalAngleLower, verticalAngleHigher);

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        //character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);

        Mathf.Lerp(-100, 100, mouseLook.x);
        if (mouseLook.x < -100)
        {
            
            character.GetComponent<Animator>().SetFloat("Turn", (float)mouseLook.x/250);
        }
        if (mouseLook.x > 100)
        {
            character.GetComponent<Animator>().SetFloat("Turn", (float)mouseLook.x / 250);
        }
    }
}
