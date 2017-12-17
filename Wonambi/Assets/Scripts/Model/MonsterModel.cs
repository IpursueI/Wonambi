﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(HitReaction))]
public class MonsterModel : MonoBehaviour {
    //hp
    public int hp;
    public ParticleSystem particleOne;
    public ParticleSystem particleZero;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D bc2d;
    private Animator anim;
    private HitReaction hitReact;
    private bool isDead;
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        hitReact = GetComponent<HitReaction>();
        isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnHitDone()
    {
        anim.SetBool("hit", false);
    }

    public void OnHit()
    {
        --hp;
        if(hp <= 0) {
            Die();
            return;
        }
        hitReact.Begin(Color.red);
        StartCoroutine(HitCoroutine());
    }

    public IEnumerator HitCoroutine()
    {
        yield return new WaitForSeconds(DefineNumber.HitBlinkDuration * 4);
        hitReact.End();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        if(collision.tag == "PlayerBullet") {
            OnHit();
        }
    }

    private void Die() {
        spriteRenderer.enabled = false;
        bc2d.enabled = false;
        particleZero.Play();
        particleOne.Play();
        isDead = true;
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(DefineNumber.DieDuration);
        Destroy(gameObject);
    }
}