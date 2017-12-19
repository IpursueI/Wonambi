using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SavePointController : MonoBehaviour {

    public GameMgr gameMgr;
    private bool isTrigger = false;
	// Use this for initialization
	void Start () 
    {
        gameMgr = GameObject.Find("GameDirector").GetComponent<GameMgr>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        // TODO 
        if(isTrigger && Input.GetKey(KeyCode.K)) {
            gameMgr.SetSavePoint(transform.position);
        }

        // TODO Show something
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("[SavePointController] OnTriggerEnter2D.");
        isTrigger = true;
    }
}
