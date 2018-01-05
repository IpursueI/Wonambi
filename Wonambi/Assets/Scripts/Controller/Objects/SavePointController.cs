using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SavePointController : MonoBehaviour {

    private Text text;

    // Use this for initialization
    private void Awake()
    {
        text = transform.Find("Canvas/Text").gameObject.GetComponent<Text>();
        text.gameObject.SetActive(false);
    }

    void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        text.text = "This bonfire looks \nso familiar.\nPress <color=#4299FFFF>L</color> to rest here";
        text.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        text.gameObject.SetActive(false);
    }
}
