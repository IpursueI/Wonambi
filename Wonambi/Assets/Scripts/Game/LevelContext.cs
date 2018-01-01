using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContext : MonoBehaviour {

    public string levelName;
    public Vector3 startPoint;
    public float width;
    public float height;
    public GameObject doubleJumpItem;
    public GameObject extraBulletItem;
    public GameObject binaryDoorItem;
    public bool isDoubleJumpDone = false;
    public bool isExtraBulletDone = false;
    public bool isBinaryDoorDown = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
