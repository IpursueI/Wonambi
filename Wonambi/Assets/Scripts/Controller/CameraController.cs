using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public int moveHoriz;
    public int moveVert;
    public float width;
    public float height;
	// Use this for initialization
	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (player == null) return;
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
        float x = player.transform.position.x * moveHoriz;
        float y = player.transform.position.y * moveVert;

        if( x < DefineNumber.CameraOffsetX) {
            x = DefineNumber.CameraOffsetX;
        } else if(x > width - DefineNumber.CameraOffsetX) {
            x = width - DefineNumber.CameraOffsetX;
        }

        if( y < DefineNumber.CameraOffsetY) {
            y = DefineNumber.CameraOffsetY;
        } else if( y > height - DefineNumber.CameraOffsetY) {
            y = height - DefineNumber.CameraOffsetY;
        }

        transform.position = new Vector3(x, y, transform.position.z);
	}

    public void SetPlayer(GameObject p)
    {
        player = p;
    }

    public void SetScreenSize(float w, float h) 
    {
        width = w;
        height = h;
    }
}
