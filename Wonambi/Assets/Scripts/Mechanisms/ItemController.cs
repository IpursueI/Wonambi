using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemController : MonoBehaviour {

    private SpriteRenderer sprite;
    private Tweener scaleTweener;
    private void Awake()
    {
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
        string tipsContent = "";
        if(tag == "DoubleJump")
        {
            tipsContent = "I can jump twice in the air now.";
        }
        else if(tag == "ExtraHP")
        {
            tipsContent = "One more HP abtained.";
        }
        else if(tag == "ExtraBullet")
        {
            tipsContent = "One more bullet abtained.";
        }
        LevelMgr.Instance.ShowTips(tipsContent, 1.5f);
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
