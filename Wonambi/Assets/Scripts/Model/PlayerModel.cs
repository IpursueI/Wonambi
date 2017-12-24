using System.Collections;
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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitReact = GetComponent<HitReaction>();
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
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

        }
        if (collision.tag == "MonsterBullet" && !isInvincible) {
            OnHit();
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


    public void Die()
    {
        spriteRenderer.enabled = false;
        particleZero.Play();
        particleOne.Play();
        isDead = true;
        transform.SetParent(null);
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
