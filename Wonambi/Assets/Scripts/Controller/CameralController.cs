using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameralController : MonoBehaviour {

    public GameObject player;
    public int moveHoriz;
    public int moveVert;

	// Use this for initialization
	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(player.transform.position.x > 0) {
            moveHoriz = 1;
        }else {
            moveHoriz = 0;
        }

		if (player.transform.position.y > 0) {
			moveVert = 1;
		}
		else {
            moveVert = 0;
		}

        transform.position = new Vector3(player.transform.position.x * moveHoriz, player.transform.position.y * moveVert, transform.position.z);
	}
}
