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

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        leftCheck = transform.Find("LeftCheck").gameObject;
        rightCheck = transform.Find("RightCheck").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        CheckGround();
	}

    private void Move()
    {
        if (forward) {
            rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);
            leftCheck.transform.localPosition = new Vector3(-0.3f, -0.4f, 0f);
            rightCheck.transform.localPosition = new Vector3(0.3f, -0.4f, 0f);
        } else {
            rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);
            leftCheck.transform.localPosition = new Vector3(0.3f, -0.4f, 0f);
            rightCheck.transform.localPosition = new Vector3(-0.3f, -0.4f, 0f);
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
