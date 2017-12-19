using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class FortController : MonoBehaviour {
    // Bullet
    public float cooldown;
    private float timer;
    public GameObject bullet;
    private GameObject muzzle;

	// Use this for initialization
	void Start () 
    {
        muzzle = transform.Find("Muzzle").gameObject;
        timer = cooldown;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Fire();
	}

    private void Fire()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            GameObject curBullet = Instantiate(bullet, transform) as GameObject;
            BulletController bulletCtrl = curBullet.GetComponent<BulletController>();
            bulletCtrl.Init(DefineNumber.BulletSpeed, DefineNumber.BulletDuration, muzzle.transform.position, gameObject);
            curBullet.transform.localScale = new Vector3(1f, 1f, 1f);
            curBullet.SetActive(true);
            timer = cooldown;
        }
    }

}
