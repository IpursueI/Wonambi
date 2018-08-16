using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Boss1CBulletController : MonoBehaviour {
    public float speed;
    public float duration;
    private Rigidbody2D rb2d;

	// Use this for initialization
	void Awake () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
    void Start() {
        if (gameObject.tag == "MonsterBullet") {
            StartCoroutine(Disapper());
        }
    }
	// Update is called once per frame
	void Update () {
        rb2d.velocity = new Vector2(speed, 0.0f);
	}

    public void Init(float _speed, float _duration, Vector3 _spawnPos)
    {
        speed = _speed;
        duration = _duration;
        if (transform.parent.localScale.x < 0.0f) {
            speed = -speed;
        }
        transform.position = _spawnPos;
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
