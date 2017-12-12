using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBullet : MonoBehaviour {

    private Rigidbody2D rb2d;
    public float speed;
    public float lifeTime;
    public Vector3 firePos;
	// Use this for initialization
	void Start () {
        if(transform.parent.localScale.x < 0.0f) {
            speed = -speed;
        }
        transform.position = transform.parent.GetComponent<DemoPlayer>().firePos.transform.position;
        transform.SetParent(null);
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(Die());
	}
	
	// Update is called once per frame
	void Update () {
        rb2d.velocity = new Vector2(speed, 0f);
	}

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    public void SetFirePos(Vector3 pos) 
    {
        firePos = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Player") {
            Destroy(gameObject);
        }
    }
}
