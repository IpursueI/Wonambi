using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Boss1CPlusBulletController : MonoBehaviour {

    public float speed;
    public float duration;
    private Rigidbody2D rb2d;
    private Vector2 direction2d;


    // Use this for initialization
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (gameObject.tag == "MonsterBullet") {
            StartCoroutine(Disapper());
        }
    }
    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = direction2d * speed;
    }

    public void Init(float _speed, float _duration, Vector3 _spawnPos, Vector3 _targetPos)
    {
        speed = _speed;
        duration = _duration;
        if (transform.parent.localScale.x < 0.0f) {
            speed = -speed;
        }
        transform.position = _spawnPos;
        direction2d = new Vector2(_targetPos.x - _spawnPos.x, _targetPos.y - _spawnPos.y);
        direction2d.Normalize();
        transform.SetParent(null);
    }

    IEnumerator Disapper()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            Destroy(gameObject);
            return;
        }
    }
}
