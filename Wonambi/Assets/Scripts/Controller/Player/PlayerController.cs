using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

[RequireComponent(typeof(Rigidbody2D))]
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
    private GameObject doubleJumpEffect;
    // Animator
    private Animator anim;
    // Bullet
    private float cooldown;
    public GameObject bullet;
    private GameObject muzzle;
    // model
    private PlayerModel model;
    private PlayerDialogController dialog;

    private int maxBulletNumber;
    private int bulletCount;

    private void Awake()
    {
        muzzle = transform.Find("Muzzle").gameObject;
        leftCheck = transform.Find("LeftCheck").gameObject;
        rightCheck = transform.Find("RightCheck").gameObject;
        doubleJumpEffect = transform.Find("DoubleJump").gameObject;
        rb2d = GetComponent<Rigidbody2D>();
        model = GetComponent<PlayerModel>();
        dialog = GetComponent<PlayerDialogController>();
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

    public void Init(bool _enableDoubleJump, int _maxBulletNumber, int _maxHP)
    {
        groundDistance = 0.5f;

        enableDoubleJump = _enableDoubleJump;
        maxBulletNumber = _maxBulletNumber;

        moveSpeed = DefineNumber.DefaultMoveSpeed;
        jumpSpeed = DefineNumber.DefaultJumpSpeed;
        inDoubleJump = false;
        leftGrounded = false;
        rightGrounded = false;
        cooldown = DefineNumber.FireCooldown;
        bulletCount = 0;

        model.Init(_maxHP);

        Save();
    }

    private void Move()
    {
        if (!GameMgr.Instance.IsInputEnable()) return;
        rb2d.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), rb2d.velocity.y);
        if (Input.GetButtonDown("Jump")) {
            if (!leftGrounded && !rightGrounded) {
                if (enableDoubleJump && !inDoubleJump) {
                    inDoubleJump = true;
                    DoDoubleJumpEffect();
                }
                else {
                    return;
                }
            }
            GameMgr.Instance.PlayJumpSFX();
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
            leftCheck.transform.localPosition = new Vector3(-0.36f, -0.58f, 0.0f);
            rightCheck.transform.localPosition = new Vector3(0.14f, -0.58f, 0.0f);
            dialog.Rotate(forward);
        } else if (rb2d.velocity.x > 0.0f && !forward){
            transform.localScale = new Vector3(1f, 1f, 1f);
            forward = true;
            leftCheck.transform.localPosition = new Vector3(0.14f, -0.58f, 0.0f);
            rightCheck.transform.localPosition = new Vector3(-0.36f, -0.58f, 0.0f);
            dialog.Rotate(forward);
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
        if (!GameMgr.Instance.IsInputEnable()) return;
        cooldown -= Time.deltaTime;
        if (Input.GetKey(KeyCode.J) && cooldown <= 0f && bulletCount < maxBulletNumber) {
            GameObject curBullet = Instantiate(bullet, transform) as GameObject;
            BulletController bulletCtrl = curBullet.GetComponent<BulletController>();
            bulletCtrl.Init(DefineNumber.BulletSpeed, 0, GetMuzzlePos(), gameObject);
            curBullet.SetActive(true);
            cooldown = DefineNumber.FireCooldown;
            GameMgr.Instance.PlayAttackSFX();
            bulletCount++;
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

    private void DoDoubleJumpEffect()
    {
        GameObject go = Instantiate(BundleMgr.Instance.GetObject("DoubleJumpEffect"));
        go.transform.position = doubleJumpEffect.transform.position;
        go.SetActive(true);
        go.transform.SetParent(null);
    }

    public bool GetDoubleJump()
    {
        return inDoubleJump;
    }

    public void EnableDoubleJump()
    {
        enableDoubleJump = true;
        LevelMgr.Instance.OnEnableDoubleJump();
    }

    public void AddBulletNumber()
    {
        maxBulletNumber++;
        LevelMgr.Instance.OnEnableExtraBullet();

    }
    public void OnBulletDestroy()
    {
        bulletCount--;
        if (bulletCount <= 0) bulletCount = 0;
    }

    public int GetPlayerMaxBulletNumber()
    {
        return maxBulletNumber;
    }
   
    public void Save()
    {
        PlayerPrefs.SetInt(PrefsKey.PlayerBulletNumber, maxBulletNumber);
        PlayerPrefs.SetInt(PrefsKey.PlayerEnableDoubleJump, enableDoubleJump ? 1 : 0);
        PlayerPrefs.SetInt(PrefsKey.PlayerMaxHP, model.GetMaxHP());
    }

    public void OnTriggerSave()
    {
        model.FullHP();
        Save();
    }
}
