using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour {

    private MovingPlatformController moveController;

    private void Awake()
    {
        moveController = transform.parent.gameObject.GetComponent<MovingPlatformController>();
    }
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") {
            Debug.Log("[PlatformTrigger] OnTriggerEnter2D.");
            moveController.OnTrigger();
            collision.gameObject.transform.SetParent(moveController.gameObject.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player") {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
