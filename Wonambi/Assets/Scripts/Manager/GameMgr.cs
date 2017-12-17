using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour {

    private LevelLoader levelLoader;
    private GameObject player;
	// Use this for initialization
	void Start () 
    {
        BundleMgr.Instance.LoadBundles();
        levelLoader = GameObject.Find("LevelContainer").GetComponent<LevelLoader>();

        // TODO test
        StartLevel("levelMap1");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartLevel(string levelName) 
    {
        levelLoader.LoadLevel("levelMap1");
        SpawnPlayer();

    }

    void SpawnPlayer() 
    {
        if(player == null) {
            // TODO 以后需要根据存盘来决定位置
            player = Instantiate(BundleMgr.Instance.GetObject("Player"), 
                                 new Vector3(levelLoader.startPoint.x, levelLoader.startPoint.y, -10), 
                                 Quaternion.identity);
        }

    }
}
