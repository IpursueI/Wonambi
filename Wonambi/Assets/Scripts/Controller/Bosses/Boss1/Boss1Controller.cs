using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour {

    private Animator anim;
    public int handNum;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        handNum = 3;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnHandDie()
    {
        --handNum;
        if(handNum <= 0)
        {
            anim.SetBool("IsDead", true);
            GameMgr.Instance.OnDemoBossDie();
        }
    }

    public void OnDie()
    {
        Destroy(gameObject);
    }
}
