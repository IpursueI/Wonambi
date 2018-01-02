using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PatrolController : MonsterController {

    // Movement
    public float speed;
    public bool forward;
    public float groundDistance;
    public float wallDistance;
    public LayerMask groundLayer;

    private Rigidbody2D rb2d;
    private GameObject leftCheck;
    private GameObject rightCheck;
    private bool leftGrounded;
    private bool rightGrounded;
    private Animator anim;

    // Bullet
    public float cooldown;
    private float timer;
    public GameObject bullet;
    private GameObject muzzle;

    private bool isFire;

    // Const
    private float fireTriggerDistance;
    private float moveTriggerDistance;

    // Model
    private MonsterModel model;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        leftCheck = transform.Find("LeftCheck").gameObject;
        rightCheck = transform.Find("RightCheck").gameObject;
        anim = GetComponent<Animator>();
        muzzle = transform.Find("Muzzle").gameObject;
        timer = cooldown;
        isFire = false;
        model = GetComponent<MonsterModel>();
	}

    // Update is called once per frame
    void FixedUpdate () {
        if(model.IsDead()) {
            return;
        }
        if (LevelMgr.Instance.IsPlayerSoClose(transform.position)) {
            if (!isFire) {
                anim.SetInteger("status", 1);
                rb2d.velocity = Vector2.zero;
                isFire = true;
            }
            // Fire
            ForwardToPlayer();
            Fire();
        } else if (LevelMgr.Instance.IsPlayerClose(transform.position)) {
            if (isFire)
            {
                anim.SetInteger("status", 0);
                isFire = false;
            }
            Move();
            CheckGround();
        } else {
            if(isFire) {
                anim.SetInteger("status", 0);
                isFire = false;
            }
            rb2d.velocity = Vector2.zero;
        }
	}

    private void Move()
    {
        if (forward) {
            rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            transform.localScale = new Vector3(1f, 1f, 1f);
            leftCheck.transform.localPosition = new Vector3(-0.5f, -0.4f, 0.0f);
            rightCheck.transform.localPosition = new Vector3(0.5f, -0.4f, 0.0f);
        }
        else {
            rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
            transform.localScale = new Vector3(-1f, 1f, 1f);
            leftCheck.transform.localPosition = new Vector3(0.5f, -0.4f, 0.0f);
            rightCheck.transform.localPosition = new Vector3(-0.5f, -0.4f, 0.0f);
        }
    }

    private void CheckGround()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(rightCheck.transform.position, Vector2.down, groundDistance, groundLayer);
        RaycastHit2D wallRight = Physics2D.Raycast(rightCheck.transform.position, Vector2.right, wallDistance, groundLayer);
        if ((forward && wallRight.transform != null) || hitRight.transform == null) {
            forward = false;
        }

        RaycastHit2D hitLeft = Physics2D.Raycast(leftCheck.transform.position, Vector2.down, groundDistance, groundLayer);
        RaycastHit2D wallLeft = Physics2D.Raycast(leftCheck.transform.position, Vector2.left, wallDistance, groundLayer);
        if ((!forward && wallLeft.transform != null) || hitLeft.transform == null) {
            forward = true;
        }
    }

    private void ForwardToPlayer()
    {
        bool isRight = LevelMgr.Instance.IsPlayerRight(transform.position);
        if(isRight && !forward) {
            transform.localScale = new Vector3(1f, 1f, 1f);
            forward = true;
            leftCheck.transform.localPosition = new Vector3(-0.5f, -0.4f, 0.0f);
            rightCheck.transform.localPosition = new Vector3(0.5f, -0.4f, 0.0f);
        } else if(!isRight && forward) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            forward = false;
            leftCheck.transform.localPosition = new Vector3(0.5f, -0.4f, 0.0f);
            rightCheck.transform.localPosition = new Vector3(-0.5f, -0.4f, 0.0f);
        }
    }

    private void Fire()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            GameObject curBullet = Instantiate(bullet, transform) as GameObject;
            BulletController bulletCtrl = curBullet.GetComponent<BulletController>();
            bulletCtrl.Init(DefineNumber.BulletSpeed, DefineNumber.BulletDuration, muzzle.transform.position, gameObject);
            curBullet.transform.localScale = new Vector3(1f, 1f, 1f);
            curBullet.SetActive(true);
            timer = cooldown;
        }
    }

    public override void OnDie()
    {
        rb2d.velocity = Vector2.zero;
    }
}
