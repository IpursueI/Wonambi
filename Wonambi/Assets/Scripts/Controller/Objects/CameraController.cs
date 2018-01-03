using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;
using DG.Tweening;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public int moveHoriz;
    public int moveVert;
    public float width;
    public float height;
    public Camera mainCamera;
    private float offsetX;
    private float offsetY;
	// Use this for initialization
	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GetComponent<Camera>();
        offsetX = DefineNumber.CameraOffsetX;
        offsetY = DefineNumber.CameraOffsetY;
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

        if( x < offsetX) {
            x = offsetX;
        } else if(x > width - offsetX - 1) {
            x = width - offsetX - 1;
        }

        if( y < offsetY) {
            y = offsetY;
        } else if( y > height - offsetY - 1) {
            y = height - offsetY - 1;
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

    public void SetOrthoSize(float o)
    {
        if (mainCamera.orthographicSize == o) return;
        mainCamera.orthographicSize = o;
        float aspectRatio = Screen.width * 1.0f / Screen.height;
        offsetY = mainCamera.orthographicSize;
        offsetX = offsetY * aspectRatio;
    }
}
