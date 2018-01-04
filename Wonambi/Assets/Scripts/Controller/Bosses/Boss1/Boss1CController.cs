using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

[RequireComponent(typeof(HitReaction))]
public class Boss1CController : MonoBehaviour {
    // Bullet
    public float cooldown;
    private int bulletNum;
    private float timer;
    public GameObject bullet;
    private GameObject muzzle;
    private bool isTrigger;

    //hp
    public int hp;
    private HitReaction hitReact;
    private bool isDead = false;
    public ParticleSystem particleOne;
    public ParticleSystem particleZero;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D bc2d;
	// Use this for initialization
	void Start () {
        muzzle = transform.Find("Muzzle").gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
        timer = cooldown;
        hitReact = GetComponent<HitReaction>();
        isDead = false;
        StartCoroutine(Trigger());
	}
	
	// Update is called once per frame
	void Update () {
        if (!isTrigger || isDead) return;
        Fire();
	}

    private void Fire() 
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            GameObject curBullet = Instantiate(bullet, transform) as GameObject;
            Boss1CBulletController bulletCtrl = curBullet.GetComponent<Boss1CBulletController>();
            bulletCtrl.Init(-DefineNumber.BulletSpeed, DefineNumber.BulletDuration, muzzle.transform.position);
            curBullet.transform.SetParent(null);
            curBullet.transform.localScale = new Vector3(1f, 1f, 1f);
            curBullet.SetActive(true);
            ++bulletNum;
            if(bulletNum >= 3) {
                timer = cooldown;
                bulletNum = 0;
            } else {
                timer = 0.1f;
            }

        }
    }

    IEnumerator Trigger() {
        yield return new WaitForSeconds(2.0f);
        isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        if (collision.tag == "PlayerBullet") {
            OnHit();
        }
    }

    public void OnHit()
    {
        --hp;
        if (hp <= 0) {
            Die();
            return;
        }
        GameMgr.Instance.PlayerMonsterHitSFX();
        hitReact.Begin(Color.red);
        StartCoroutine(HitCoroutine());
    }

    public IEnumerator HitCoroutine()
    {
        yield return new WaitForSeconds(DefineNumber.HitBlinkDuration * 4);
        hitReact.End();
    }

    private void Die()
    {
        spriteRenderer.enabled = false;
        bc2d.enabled = false;
        particleZero.Play();
        particleOne.Play();
        isDead = true;
        GameMgr.Instance.PlayMonsterDieSFX();
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(DefineNumber.DieDuration);
        Destroy(gameObject);
    }
}
