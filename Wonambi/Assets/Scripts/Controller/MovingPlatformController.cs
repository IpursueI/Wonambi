using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class MovingPlatformController : MonoBehaviour {

    public bool auto;
    public float speed;
    public bool willTurn = false;
    public Vector3 turnPosition;
    public MoveDirection turnDirection;
    public MoveDirection direction = MoveDirection.None;
    public bool isEnd;

	// Use this for initialization
	void Start () 
    {
        speed = 2.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!auto) return;
        Move();
        Turn();
	}
         
    private void Move() 
    {
        switch (direction) {
        case MoveDirection.Up:
        transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);
        break;
        case MoveDirection.Down:
        transform.localPosition += new Vector3(0, -speed * Time.deltaTime, 0);
        break;
        case MoveDirection.Left:
        transform.localPosition += new Vector3(-speed * Time.deltaTime, 0, 0);
        break;
        case MoveDirection.Right:
        transform.localPosition += new Vector3(speed * Time.deltaTime, 0, 0);
        break;
        default:
        break;
        }
    }

    private void Turn()
    {
        if (isEnd && !auto) return;
        if (!willTurn) return;
        if((turnPosition - transform.localPosition).sqrMagnitude <= DefineNumber.CloseTurnDistance * DefineNumber.CloseTurnDistance) {
            direction = turnDirection;
            willTurn = false;
            isEnd = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TurnPoint"){
            willTurn = true;
            turnPosition = collision.transform.localPosition;
            TurnPointController tpCtrl = collision.GetComponent<TurnPointController>();
            if(tpCtrl.direction1 == MoveDirection.None) {
                turnDirection = tpCtrl.direction2;
                isEnd = true;
            } else {
                if(tpCtrl.direction2 == MoveDirection.None) {
                    turnDirection = tpCtrl.direction1;
                    isEnd = true;
                } else {
                    turnDirection = PickOneDirection(tpCtrl.direction1, tpCtrl.direction2, direction);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            collision.gameObject.transform.SetParent(null);
        }        
    }

    private MoveDirection PickOneDirection(MoveDirection direction1, MoveDirection direction2, MoveDirection curDirection) 
    {
        if(curDirection == MoveDirection.None) {
            return direction1;
        }
        if(curDirection == MoveDirection.Up) {
            if(direction1 == MoveDirection.Down) {
                return direction2;
            } else {
                return direction1;
            }
        }
        if (curDirection == MoveDirection.Down) {
            if (direction1 == MoveDirection.Up) {
                return direction2;
            }
            else {
                return direction1;
            }
        }
        if (curDirection == MoveDirection.Left) {
            if (direction1 == MoveDirection.Right) {
                return direction2;
            }
            else {
                return direction1;
            }
        }
        if (curDirection == MoveDirection.Right) {
            if (direction1 == MoveDirection.Left) {
                return direction2;
            }
            else {
                return direction1;
            }
        }
        return curDirection;
    }
}