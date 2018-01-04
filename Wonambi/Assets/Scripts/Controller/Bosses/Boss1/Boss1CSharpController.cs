using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class Boss1CSharpController : MonoBehaviour {

    private GameObject bullet; 

    //hp
    public int hp;
    private HitReaction hitReact;
    private bool isDead = false;
    public ParticleSystem particleOne;
    public ParticleSystem particleZero;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D bc2d;

    public Boss1Controller bodyController;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
        hitReact = GetComponent<HitReaction>();
        isDead = false;
        bullet = transform.Find("Sharp").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Fire()
    {
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
        if(bullet != null){
            bullet.SetActive(false);
        }
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
        bodyController.OnHandDie();\






    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "PlayerBullet") {
            GameMgr.Instance.PlayerMonsterHitSFX();
        }
    }
}
