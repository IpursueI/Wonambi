using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTriggerController : MonoBehaviour {

    private Animator anim;
    private bool isTriggered;
    public int mark;
    public BinaryDoorController controller;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init() 
    {
        anim = GetComponent<Animator>();
        if(mark == 3 || mark == 2) {
            isTriggered = true;
        } else {
            isTriggered = false;
        }
        anim.SetBool("isTrigger", isTriggered);
    }

    public bool IsTriggered()
    {
        return isTriggered;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (controller.isSolve) return;
        if(collision.tag == "PlayerBullet") {
            isTriggered = !isTriggered;
            anim.SetBool("isTrigger", isTriggered);
            controller.OnTriggerHit();
        }
    }

    public void SetTrigger(bool isOn)
    {
        if(isOn)
        {
            isTriggered = true;
        }
        else
        {
            isTriggered = false;
        }
        anim.SetBool("isTrigger", isTriggered);
    }
}
