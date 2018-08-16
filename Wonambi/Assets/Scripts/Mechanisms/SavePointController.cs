using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SavePointController : MonoBehaviour {

    private Text text;
    private bool isTrigger;

    // Use this for initialization
    private void Awake()
    {
        text = transform.Find("Canvas/Text").gameObject.GetComponent<Text>();
        text.gameObject.SetActive(false);
        isTrigger = false;
    }

    void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
        CheckSaveTrigger();
	}

    private void CheckSaveTrigger()
    {
        if (!GameMgr.Instance.IsInputEnable()) return;
        if (isTrigger && Input.GetKeyDown(KeyCode.L)) {
            GameMgr.Instance.PlayBonfireSFX();
            text.text = "Game saved";
            isTrigger = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        text.text = "\nPress <color=#4299FFFF>L</color> to save";
        text.gameObject.SetActive(true);
        isTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        text.gameObject.SetActive(false);
    }
}
