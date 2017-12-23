using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class FortController : MonsterController {
    // Bullet
    public float cooldown;
    private float timer;
    public GameObject bullet;
    private GameObject muzzle;
    private MonsterModel model;
    private float fireTriggerDistance;
    private bool forward;

	// Use this for initialization
	void Start () 
    {
        muzzle = transform.Find("Muzzle").gameObject;
        timer = cooldown;
        model = GetComponent<MonsterModel>();
        fireTriggerDistance = DefineNumber.MonsterMoveTriggerDistance * DefineNumber.MonsterMoveTriggerDistance;
        forward = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (model.IsDead()) return;
        float distance = LevelMgr.Instance.DistanceToPlayer(transform.position);
        if (distance < 0.0f) return;
        if (distance < fireTriggerDistance) {
            ForwardToPlayer();
            Fire();
        }
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

    private void ForwardToPlayer()
    {
        bool isRight = LevelMgr.Instance.IsPlayerRight(transform.position);
        if (isRight && !forward) {
            transform.localScale = new Vector3(1f, 1f, 1f);
            forward = true;
        }
        else if (!isRight && forward) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            forward = false;
        }
    }

}
