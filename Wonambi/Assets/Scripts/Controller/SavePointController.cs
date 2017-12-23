using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SavePointController : MonoBehaviour {

    private bool isTrigger = false;
    private Text text;
    private Animator anim;
    // Use this for initialization
    private void Awake()
    {
        text = transform.Find("Canvas/Text").gameObject.GetComponent<Text>();
        text.gameObject.SetActive(false);
        anim = GetComponent<Animator>();
    }

    void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
        if(isTrigger && Input.GetKey(KeyCode.L)) {
            LevelMgr.Instance.RebornPlayer(transform.position);
            text.text = "Game saved.";
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        isTrigger = true;
        text.text = "It is an old school floppy disk.\nPress L to save game.";
        text.gameObject.SetActive(true);
        anim.SetBool("isTrigger", isTrigger);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        isTrigger = false;
        text.gameObject.SetActive(false);
        anim.SetBool("isTrigger", isTrigger);
    }
}
