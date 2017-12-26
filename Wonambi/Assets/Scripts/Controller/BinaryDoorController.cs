using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class BinaryDoorController : MonoBehaviour {

    private List<GameObject> triggers = new List<GameObject>(); 
    private GameObject doorOpen;
    private GameObject doorClose;

	// Use this for initialization
	void Start () {
        for (int i = 1; i <= 4; ++i) {
            GameObject bt = transform.Find("Trigger" + i.ToString()).gameObject;
            bt.GetComponent<BinaryTriggerController>().Init();
            triggers.Add(bt);
        }
        doorOpen = transform.Find("DoorOpen").gameObject;
        doorClose = transform.Find("DoorClose").gameObject;
        doorOpen.SetActive(false);
        doorClose.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerHit() {
        int rightNum = 0;
        for (int i = 1; i <= 4; ++i) {
            if(i == 3) {
                if(!triggers[i-1].GetComponent<BinaryTriggerController>().IsTriggered()) {
                    ++rightNum;
                }
            } else {
                if(triggers[i-1].GetComponent<BinaryTriggerController>().IsTriggered()) {
                    ++rightNum;
                }
            }
        }
        if(rightNum >= 4) {
            doorOpen.SetActive(true);
            doorClose.SetActive(false);
        } else {
            doorOpen.SetActive(false);
            doorClose.SetActive(true);
        }
    }
}
