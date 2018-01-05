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
        yield return new WaitForSeconds(0.5f);
        tSequence = DOTween.Sequence();
        tSequence.AppendInterval(5.0f);
        tSequence.Append(transform.DOMove(new Vector3(12.0f, 7.0f, -1.0f), 3.0f));
        tSequence.AppendInterval(5.0f);
        tSequence.Append(transform.DOMove(new Vector3(7.0f, 11.0f, -1.0f), 3.0f));
        tSequence.AppendInterval(5.0f);
        tSequence.SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerBullet")
        {
            GameMgr.Instance.PlayerInvincibleMonsterHitSFX();
        }
    }
}
