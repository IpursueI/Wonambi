﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HitReaction))]
public class PlayerModel : MonoBehaviour {
    
    public int hp;
    public int maxHp;
    public ParticleSystem particleOne;
    public ParticleSystem particleZero;
    private SpriteRenderer spriteRenderer;
    private HitReaction hitReact;
    public bool isDead;
    private bool isInvincible;
    private GameDirector gameDirector;
    public int status;
    public PlayerController controller;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitReact = GetComponent<HitReaction>();
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        controller = GetComponent<PlayerController>();
    }
    // Use this for initialization
    void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(int _maxHp) {
        isDead = false;
        isInvincible = false;
        maxHp = _maxHp;
        hp = maxHp;
        spriteRenderer.enabled = true;
        LevelMgr.Instance.RefreshHP();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isDead) return;
        if (isInvincible) return;
        if(collision.tag == "Monster") {
            OnHit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        if(collision.tag == "Heart") {
            AddHP(1);
            Destroy(collision.gameObject);
            gameDirector.GetAudio().PlayItem();

        }
        if (collision.tag == "MonsterBullet" && !isInvincible) {
            OnHit();
        }
        if (collision.tag == "SavePoint") {
            status |= PlayerStatus.InBonfire;
        }
        if(collision.tag == "DoubleJump") {
            controller.EnableDoubleJump();
            gameDirector.GetAudio().PlayItem();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isDead) return;
        if(collision.tag == "SavePoint") {
            status &= ~PlayerStatus.InBonfire;
        }
    }

    private void OnHit()
    {
        --hp;
        LevelMgr.Instance.RefreshHP();
        if(hp <= 0) {
            Die();
            return;
        }
        gameDirector.GetAudio().PlayPlayerHit();
        spriteRenderer.color = Color.grey;
        hitReact.Begin(Color.white);
        isInvincible = true;
        StartCoroutine(HitCoroutine());
    }

    public IEnumerator HitCoroutine()
    {
        yield return new WaitForSeconds(DefineNumber.InvincibleDuration);
        isInvincible = false;
        hitReact.End();
        spriteRenderer.color = Color.green;
    }


    public void Die()
    {
        spriteRenderer.enabled = false;
        particleZero.Play();
        particleOne.Play();
        isDead = true;
        transform.SetParent(null);
        gameDirector.GetAudio().PlayPlayerDie();
        StartCoroutine(DieCoroutine());
    }

    public IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(DefineNumber.DieDuration);
        LevelMgr.Instance.RestartLevel();
    }

    public bool UnAttackAble()
    {
        return isInvincible || isDead;
    }

    private void AddHP(int v)
    {
        hp += v;
        LevelMgr.Instance.RefreshHP();
        if(hp > maxHp) {
            hp = maxHp;
        }
    }

    public void FullHP()
    {
        AddHP(maxHp);
    }

    public int GetMaxHP()
    {
        return maxHp;
    }
}
