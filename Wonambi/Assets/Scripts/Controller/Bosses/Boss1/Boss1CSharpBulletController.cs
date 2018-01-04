using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;
using DG.Tweening;

public class Boss1CSharpBulletController : MonoBehaviour {

    private Sequence tSequence;
	// Use this for initialization
	void Start () {
        StartCoroutine(MoveCoroutine());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator MoveCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        tSequence = DOTween.Sequence();
        tSequence.AppendInterval(3.0f);
        tSequence.Append(transform.DOMove(new Vector3(10.0f, 10.0f, -1.0f), 3.0f));
        tSequence.AppendInterval(5.0f);
        tSequence.Append(transform.DOMove(new Vector3(5.0f, 5.0f, -1.0f), 3.0f));
        tSequence.SetLoops(-1, LoopType.Yoyo);
    }
}
