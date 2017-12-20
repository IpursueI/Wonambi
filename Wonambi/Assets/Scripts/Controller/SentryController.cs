using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class SentryController : MonoBehaviour {

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
    private GameMgr gameMgr;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        leftCheck = transform.Find("LeftCheck").gameObject;
        rightCheck = transform.Find("RightCheck").gameObject;
        gameMgr = GameObject.Find("GameDirector").GetComponent<GameMgr>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        /*
        if(!gameMgr.IsPlayerClose(transform.position)) {
            rb2d.velocity = new Vector2(0, 0);
            return;
        }
        */
        Move();
        CheckGround();
	}

    private void Move()
    {
        if (forward) {
            rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
        } else {
            rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
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
}
