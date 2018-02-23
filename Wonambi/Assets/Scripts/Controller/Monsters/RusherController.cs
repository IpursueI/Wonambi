using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class RusherController : MonsterController
{
    // Movement
    public float speed;
    public bool forward;
    public LayerMask groundLayer;
    private float groundDistance;
    private float wallDistance;
    private GameObject leftCheck;
    private GameObject rightCheck;
    private Rigidbody2D rb2d;
    private MonsterModel model;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        model = GetComponent<MonsterModel>();
        leftCheck = transform.Find("LeftCheck").gameObject;
        rightCheck = transform.Find("RightCheck").gameObject;
        forward = LevelMgr.Instance.IsPlayerRight(transform.position);
        groundDistance = 0.2f;
        wallDistance = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (model.IsDead()) {
            return;
        }
        if (!LevelMgr.Instance.IsPlayerClose(transform.position)) {
            rb2d.velocity = Vector2.zero;
            return;
        }

        Move();
        CheckGround();
    }

    private void Move()
    {
        if (forward) {
            rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            transform.localScale = new Vector3(1f, 1f, 1f);
            leftCheck.transform.localPosition = new Vector3(-0.45f, -0.6f, 0.0f);
            rightCheck.transform.localPosition = new Vector3(0.15f, -0.6f, 0.0f);
        }
        else {
            rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
            transform.localScale = new Vector3(-1f, 1f, 1f);
            leftCheck.transform.localPosition = new Vector3(0.15f, -0.6f, 0.0f);
            rightCheck.transform.localPosition = new Vector3(0.45f, -0.6f, 0.0f);
        }
    }

    private void CheckGround()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(leftCheck.transform.position, Vector2.down, groundDistance, groundLayer);
        if (hitLeft.transform != null && forward) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
        else {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -5);
        }
        RaycastHit2D hitRight = Physics2D.Raycast(rightCheck.transform.position, Vector2.down, groundDistance, groundLayer);
        if (hitRight.transform != null && !forward) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
        else {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -5);
        }

        /*
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
        */
    }

    public override void OnDie()
    {
        rb2d.velocity = Vector2.zero;
    }
}
