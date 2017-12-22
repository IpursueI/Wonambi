using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerWithRigidBodyController : MonoBehaviour {

    // Movement
    public bool forward;
    public LayerMask groundLayer;
    private float groundDistance;
    private bool enableDoubleJump;
    private float moveSpeed;
    private float jumpSpeed;
    private GameObject leftCheck;
    private GameObject rightCheck;
    private Rigidbody2D rb2d;
    private bool inDoubleJump;
    private bool leftGrounded;
    private bool rightGrounded;
    // Animator
    private Animator anim;
    // Bullet
    private float cooldown;
    public GameObject bullet;
    private GameObject muzzle;
    // model
    private PlayerModel model;

    private void Awake()
    {
        muzzle = transform.Find("Muzzle").gameObject;
        leftCheck = transform.Find("LeftCheck").gameObject;
        rightCheck = transform.Find("RightCheck").gameObject;
        rb2d = GetComponent<Rigidbody2D>();
        model = GetComponent<PlayerModel>();
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () 
    {

	}

    private void Update()
    {
        if (model.isDead) 
        {
            rb2d.velocity = Vector2.zero;
            return;
        }
        Move();
        Rotate();
        CheckGround();
        Fire();
        SyncAnimator();
        CheckDie();
    }

    public void Init()
    {
        groundDistance = 0.5f;
        //enableDoubleJump = PlayerPrefs.GetInt(PrefsKey.PlayerEnableDoubleJump, 0) > 0;
        enableDoubleJump = true;
        moveSpeed = PlayerPrefs.GetFloat(PrefsKey.PlayerMoveSpeed, DefineNumber.DefaultMoveSpeed);
        jumpSpeed = PlayerPrefs.GetFloat(PrefsKey.PlayerJumpSpeed, DefineNumber.DefaultJumpSpeed);
        inDoubleJump = true;
        leftGrounded = false;
        rightGrounded = false;
        cooldown = DefineNumber.FireCooldown;
    }

    public void Save()
    {
        if(enableDoubleJump) {
            PlayerPrefs.SetInt(PrefsKey.PlayerEnableDoubleJump, 1);
        } else {
            PlayerPrefs.SetInt(PrefsKey.PlayerEnableDoubleJump, 0);
        }
        PlayerPrefs.SetFloat(PrefsKey.PlayerMoveSpeed, moveSpeed);
        PlayerPrefs.SetFloat(PrefsKey.PlayerJumpSpeed, jumpSpeed);
    }

    private void Move()
    {
        rb2d.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), rb2d.velocity.y);
        if (Input.GetButtonDown("Jump")) {
            if (!leftGrounded && !rightGrounded) {
                if (enableDoubleJump && !inDoubleJump) {
                    inDoubleJump = true;
                }
                else {
                    return;
                }
            }
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed * Vector2.up.y);
        }
        if (rb2d.velocity.y < 0) {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * DefineNumber.FallFactor * Time.deltaTime;
        }
        else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * DefineNumber.LowJumpFactor * Time.deltaTime;
        }
        if(rb2d.velocity.y < DefineNumber.MaxFallSpeed) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, DefineNumber.MaxFallSpeed);
        }
    }

    private void Rotate()
    {
        if (rb2d.velocity.x < 0.0f && forward) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            forward = false;
            leftCheck.transform.localPosition = new Vector3(-0.45f, -0.6f, 0.0f);
            rightCheck.transform.localPosition = new Vector3(0.05f, -0.6f, 0.0f);
        } else if (rb2d.velocity.x > 0.0f && !forward){
            transform.localScale = new Vector3(1f, 1f, 1f);
            forward = true;
            leftCheck.transform.localPosition = new Vector3(0.05f, -0.6f, 0.0f);
            rightCheck.transform.localPosition = new Vector3(-0.45f, -0.6f, 0.0f);
        }    
    }

    private void CheckGround()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(leftCheck.transform.position, Vector2.down, groundDistance, groundLayer);
        if (hitLeft.transform != null) {
            leftGrounded = true;
            inDoubleJump = false;
        }
        else {
            leftGrounded = false;
        }
        RaycastHit2D hitRight = Physics2D.Raycast(rightCheck.transform.position, Vector2.down, groundDistance, groundLayer);
        if (hitRight.transform != null) {
            rightGrounded = true;
            inDoubleJump = false;
        }
        else {
            rightGrounded = false;
        }
    }

    private void Fire()
    {
        cooldown -= Time.deltaTime;
        if (Input.GetKey(KeyCode.J) && cooldown <= 0f) {
            GameObject curBullet = Instantiate(bullet, transform) as GameObject;
            BulletController bulletCtrl = curBullet.GetComponent<BulletController>();
            bulletCtrl.Init(DefineNumber.BulletSpeed, DefineNumber.BulletDuration, GetMuzzlePos(), gameObject);
            curBullet.SetActive(true);
            cooldown = DefineNumber.FireCooldown;
        }
    }

    private void SyncAnimator()
    {
        anim.SetFloat("speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetBool("grounded", leftGrounded || rightGrounded);
        anim.SetFloat("upSpeed", rb2d.velocity.y);
    }

    public Vector3 GetMuzzlePos()
    {
        return muzzle.transform.position;
    }

    private void CheckDie()
    {
        if(transform.position.y < DefineNumber.PlayerMinY) {
            model.Die();
        }
    }
}
