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
    public int status;
    public PlayerController controller;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitReact = GetComponent<HitReaction>();
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
        if(collision.tag == "Monster" || collision.tag == "InvincibleMonsterBullet") {
            OnHit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        if(collision.tag == "Heart") {
            AddHP(1);
            Destroy(collision.gameObject);
            GameMgr.Instance.PlayPickSFX();

        }
        if (collision.tag == "MonsterBullet") {
            if(!isInvincible) {
                OnHit();
            }
        }
        if (collision.tag == "SavePoint") {
            status |= PlayerStatus.InBonfire;
        }
        if(collision.tag == "DoubleJump") {
            controller.EnableDoubleJump();
            GameMgr.Instance.PlayPickSFX();
        }
        if(collision.tag == "ExtraBullet")
        {
            controller.AddBulletNumber();
            GameMgr.Instance.PlayPickSFX();
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
        GameMgr.Instance.PlayPlayerHitSFX();
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
        GameMgr.Instance.PlayPlayerDieSFX();
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
