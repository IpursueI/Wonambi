using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class BulletController : MonoBehaviour {
    public float speed;
    public float duration;
    private Vector3 spawnPos;
    private Rigidbody2D rb2d;
    private GameObject owner;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    // Use this for initialization
    void Start () 
    {
        StartCoroutine(Die());
	}
	
	// Update is called once per frame
	void Update () 
    {
        rb2d.velocity = new Vector2(speed, 0);
	}

    public void Init(float _speed, float _duration, Vector3 _spawnPos, GameObject _owner)
    {
        speed = _speed;
        duration = _duration;
        if (transform.parent.localScale.x < 0.0f) {
            speed = -speed;
        }
        transform.position = _spawnPos;
        owner = _owner;
        transform.SetParent(null);
    }

    IEnumerator Die() 
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.tag == "PlayerBullet"){
            if(collision.gameObject.tag == "MonsterBullet" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "TurnPoint") {
                return;
            }
        } else if(gameObject.tag == "MonsterBullet"){
            if (collision.gameObject.tag == "Monster") return;
            if (collision.gameObject.tag == "Player") {
                var playerModel = collision.gameObject.GetComponent<PlayerModel>();
                if (playerModel.UnAttackAble()) return;
            }
        }
        Destroy(gameObject);
       
    }
}
