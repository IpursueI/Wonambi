using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HitReaction))]
public class PlayerModel : MonoBehaviour {
    
    public int hp;
    public ParticleSystem particleOne;
    public ParticleSystem particleZero;
    private SpriteRenderer spriteRenderer;
    private HitReaction hitReact;
    private bool isDead;
    private bool isInvincible;
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitReact = GetComponent<HitReaction>();
        isDead = false;
        isInvincible = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isDead) return;
        if (isInvincible) return;
        if(collision.gameObject.tag == "Monster") {
            OnHit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        if (isInvincible) return;
        if (collision.gameObject.tag == "MonsterBullet") {
            OnHit();
        }
    }
    private void OnHit()
    {
        --hp;
        if(hp <= 0) {
            Die();
            return;
        }

        hitReact.Begin(Color.white);
        isInvincible = true;
        StartCoroutine(HitCoroutine());
    }

    public IEnumerator HitCoroutine()
    {
        yield return new WaitForSeconds(DefineNumber.InvincibleDuration);
        isInvincible = false;
        hitReact.End();
    }


    private void Die()
    {
        spriteRenderer.enabled = false;
        particleZero.Play();
        particleOne.Play();
        isDead = true;
        StartCoroutine(DieCoroutine());
    }

    public IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(DefineNumber.DieDuration);

    }

    private void Reborn()
    {
        Debug.Log("[PlayerModel] Reborn.");
    }

    public bool UnAttackAble()
    {
        return isInvincible || isDead;
    }
}
