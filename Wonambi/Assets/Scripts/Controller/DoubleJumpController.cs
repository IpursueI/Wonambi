using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DoubleJumpController : MonoBehaviour {

    private Text text;
    private SpriteRenderer sprite;

    private void Awake()
    {
        text = transform.Find("Canvas/Text").gameObject.GetComponent<Text>();
        text.gameObject.SetActive(false);
        sprite = GetComponent<SpriteRenderer>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ShowTips() {
        sprite.enabled = false;
        text.gameObject.SetActive(true);
        text.DOFade(0.0f, 0.5f);
        transform.DOMoveY(transform.position.y + 1, 0.5f).OnComplete(EndTween);
    }

    private void EndTween() {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") {
            ShowTips();
        }   
    }
}
