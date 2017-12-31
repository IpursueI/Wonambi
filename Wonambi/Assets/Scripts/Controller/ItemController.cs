using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemController : MonoBehaviour {

    private Text text;
    private SpriteRenderer sprite;
    private Tweener scaleTweener;
    private void Awake()
    {
        text = transform.Find("Canvas/Text").gameObject.GetComponent<Text>();
        text.gameObject.SetActive(false);
        sprite = GetComponent<SpriteRenderer>();
        
    }
    // Use this for initialization
    void Start () {
        scaleTweener = transform.DOScale(1.5f, 1f).SetLoops(-1, LoopType.Yoyo);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ShowTips() {
        scaleTweener.Kill();
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        GetComponent<BoxCollider2D>().enabled = false;
        sprite.enabled = false;
        text.gameObject.SetActive(true);
        text.DOFade(0.0f, 3f);
        transform.DOMoveY(transform.position.y + 1, 3f).OnComplete(EndTween);
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
