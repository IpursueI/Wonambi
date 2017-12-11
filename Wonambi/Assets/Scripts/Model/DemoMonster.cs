using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMonster : MonoBehaviour {

    public float speed;
    public bool moveingLeft;
    private Rigidbody2D rb;
    //public float moveTime;

    public GameObject leftCheck;
    public GameObject rightCheck;
    public float groundDistance;
    public float wallDistance;
    public bool leftGrounded;
    public bool rightGrounded;
    public LayerMask groundLayer;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        speed = 2.0f;
        //moveTime = 2.0f;
        moveingLeft = true;
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
        CheckGround();
        /*
        if (moveTime < 0.0f) {
            moveTime = 2.0f;
            moveingLeft = !moveingLeft;
        } else {
            moveTime -= Time.deltaTime;
        }
        */
	}

    private void Movement()
    {
        if(moveingLeft)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    private void CheckGround()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(leftCheck.transform.position, Vector2.down, groundDistance, groundLayer);
        RaycastHit2D wallLeft = Physics2D.Raycast(leftCheck.transform.position, Vector2.left, wallDistance, groundLayer);
        if(moveingLeft && wallLeft.transform != null) {
            moveingLeft = false;
        }

        if (hitLeft.transform != null) {
            leftGrounded = true;
        }
        else {
            leftGrounded = false;
            moveingLeft = false;
        }

        RaycastHit2D hitRight = Physics2D.Raycast(rightCheck.transform.position, Vector2.down, groundDistance, groundLayer);
        RaycastHit2D wallRight = Physics2D.Raycast(rightCheck.transform.position, Vector2.right, wallDistance, groundLayer);
        if (!moveingLeft && wallRight.transform != null) {
            moveingLeft = true;
        }

        if (hitRight.transform != null) {
            rightGrounded = true;
        }
        else {
            rightGrounded = false;
            moveingLeft = true;
        }
    }
}
