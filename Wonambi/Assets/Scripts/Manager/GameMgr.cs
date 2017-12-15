using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour {

    private LevelLoader levelLoader;
	// Use this for initialization
	void Start () {
        BundleMgr.Instance.LoadBundles();
        levelLoader = GameObject.Find("Level").GetComponent<LevelLoader>();
        levelLoader.LoadLevel("level1");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
