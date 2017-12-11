using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayer : MonoBehaviour {

    public float maxSpeed;
    public float moveSpeed;
    public float jumpSpeed;

    public bool forward;

    public GameObject leftCheck;
    public GameObject rightCheck;
    public bool leftGrounded;
	public bool rightGrounded;
    public LayerMask groundLayer;
    public float groundDistance;
    public bool enableDoubleJump;
    public bool inDoubleJump;

    private Rigidbody2D rb;
    private Animator anim;

    public GameObject checkPoint;
	// Use this for initialization
	void Start () {
        checkPoint = GameObject.Find("CheckPoint");

        transform.position = checkPoint.transform.position;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();


        moveSpeed = 3.0f;
        jumpSpeed = 6.5f;
        forward = true;
        groundDistance = 0.2f;

        enableDoubleJump = true;
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
        Rotation();
        CheckGrounded();

        SyncAnimator();
        CheckPosition();
	}

    private void Movement()
    {
        rb.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), rb.velocity.y);
        if (Input.GetButtonDown("Jump")){
            if (!leftGrounded && !rightGrounded) {
                if (enableDoubleJump && !inDoubleJump) {
                    inDoubleJump = true;
                }else{
                    return;
                }
            }
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    private void Rotation()
    {
        if (rb.velocity.x < 0.0f && forward) {
            transform.localScale = new Vector3(-1, 1, 1);
            forward = false;
        } else if (rb.velocity.x > 0.0f && !forward){
            transform.localScale = new Vector3(1, 1, 1);
            forward = true;
        }        
    }

    private void SyncAnimator()
    {
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("grounded", leftGrounded || rightGrounded);
        anim.SetFloat("upSpeed", rb.velocity.y);
    }

    private void CheckGrounded()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(leftCheck.transform.position, Vector2.down, groundDistance, groundLayer);
        if(hitLeft.transform != null) {
            leftGrounded = true;
			inDoubleJump = false;
        } else {
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

    private void CheckPosition()
    {
        if (transform.position.y < -10.0f) {
            Debug.Log("[DemoPlayer] CheckPosition. transform.position.y = " + transform.position.y);
            Respawn();
        }
    }
    private void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = checkPoint.transform.position;
    }
}
