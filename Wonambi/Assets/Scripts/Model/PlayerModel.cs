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
    public bool isDead;
    private bool isInvincible;
    private PlayerWithRigidBodyController controller;
    private GameMgr gameMgr;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitReact = GetComponent<HitReaction>();
        controller = GetComponent<PlayerWithRigidBodyController>();
        gameMgr = GameObject.Find("GameDirector").GetComponent<GameMgr>();
    }
    // Use this for initialization
    void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init() {
        isDead = false;
        isInvincible = false;
        hp = PlayerPrefs.GetInt(PrefsKey.PlayerMaxHP, 3);
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


    public void Die()
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
        Reborn();
    }

    private void Reborn()
    {
        Init();
        controller.Init();
        spriteRenderer.enabled = true;
        transform.position = gameMgr.GetPlayerSpawnPos();
        transform.SetParent(null);
    }

    public bool UnAttackAble()
    {
        return isInvincible || isDead;
    }
}
