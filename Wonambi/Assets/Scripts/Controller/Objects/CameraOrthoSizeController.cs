using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrthoSizeController : MonoBehaviour {

    public float orthoSize;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("[CameraOrthoSizeController] OnTriggerEnter2D");
            GameMgr.Instance.SetCameraOrthoSize(orthoSize);
        }
    }
}
