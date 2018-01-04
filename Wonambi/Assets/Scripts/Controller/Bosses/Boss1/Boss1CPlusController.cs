using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class Boss1CPlusController : MonoBehaviour {
    // Bullet
    public float cooldown;
    private int bulletNum;
    private float timer;
    public GameObject bullet;
    private bool isTrigger;

    //hp
    public int hp;
    private HitReaction hitReact;
    private bool isDead = false;
    public ParticleSystem particleOne;
    public ParticleSystem particleZero;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D bc2d;

    private GameObject plus1;
    private GameObject plus2;

    public Boss1Controller bodyController;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
        timer = cooldown;
        hitReact = GetComponent<HitReaction>();
        isDead = false;
        plus1 = transform.Find("Plus1").gameObject;
        plus2 = transform.Find("Plus2").gameObject;
        StartCoroutine(Trigger());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTrigger || isDead) return;
        Fire();
    }

    private void Fire()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            GameObject curBullet = Instantiate(bullet, transform) as GameObject;
            Boss1CPlusBulletController bulletCtrl = curBullet.GetComponent<Boss1CPlusBulletController>();
            bulletCtrl.Init(-DefineNumber.BulletSpeed, DefineNumber.BulletDuration, plus1.transform.position, LevelMgr.Instance.GetPlayerPos());
            curBullet.transform.SetParent(null);
            curBullet.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            curBullet.SetActive(true);
            timer = cooldown;
        }
    }

    IEnumerator Trigger()
    {
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
        plus1.SetActive(false);
        plus2.SetActive(false);
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
        bodyController.OnHandDie();
    }
}
