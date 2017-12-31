using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemEffectController : MonoBehaviour {

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    // Use this for initialization
    void Start () {
        DoEffect();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void DoEffect()
    {
        sprite.DOFade(0.0f, 0.8f).OnComplete(EndTween);
    }

    private void EndTween()
    {
        Destroy(gameObject);
    }
}
