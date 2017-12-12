using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {

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

	// Use this for initialization
	void Start () 
    {
        Init();
	}
	
	// Update is called once per frame
	void Update () 
    {
        Move();
        Rotate();
        CheckGround();
        Fire();
        SyncAnimator();
	}

    public void Init()
    {
        groundDistance = 0.5f;
        enableDoubleJump = PlayerPrefs.GetInt(PrefsKey.PlayerEnableDoubleJump, 0) > 0;
        moveSpeed = PlayerPrefs.GetFloat(PrefsKey.PlayerMoveSpeed, DefineNumber.DefaultMoveSpeed);
        jumpSpeed = PlayerPrefs.GetFloat(PrefsKey.PlayerJumpSpeed, DefineNumber.DefaultJumpSpeed);
        leftCheck = transform.Find("LeftCheck").gameObject;
        rightCheck = transform.Find("RightCheck").gameObject;
        rb2d = GetComponent<Rigidbody2D>();
        inDoubleJump = false;
        leftGrounded = false;
        rightGrounded = false;
        muzzle = transform.Find("Muzzle").gameObject;
        anim = GetComponent<Animator>();
        cooldown = 0.3f;
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
    }

    private void Rotate()
    {
        if (rb2d.velocity.x < 0.0f && forward) {
            transform.localScale = new Vector3(-1, 1, 1);
            forward = false;
            leftCheck.transform.localPosition = new Vector3(-0.5f, -0.5f, 0.0f);
            rightCheck.transform.localPosition = new Vector3(0.1f, -0.5f, 0.0f);
        } else if (rb2d.velocity.x > 0.0f && !forward){
            transform.localScale = new Vector3(1, 1, 1);
            forward = true;
            leftCheck.transform.localPosition = new Vector3(0.1f, -0.5f, 0.0f);
            rightCheck.transform.localPosition = new Vector3(-0.5f, -0.5f, 0.0f);
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
            Instantiate(bullet, transform).SetActive(true);
            cooldown = 0.5f;
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
}
