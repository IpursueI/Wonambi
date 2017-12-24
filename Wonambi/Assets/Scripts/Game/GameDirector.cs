﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class GameDirector : MonoBehaviour {

    private BundleMgr bundleMgr;
    private LevelMgr levelMgr;
    private UIController uiController;
    void Awake()
    {
        bundleMgr = BundleMgr.Instance;
        bundleMgr.Init();
        levelMgr = LevelMgr.Instance;
        levelMgr.Init(this);
        uiController = GameObject.Find("UICanvas").GetComponent<UIController>();
    }

	// Use this for initialization
	void Start () {
        LevelMgr.Instance.StartNewLevel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NewGame()
    {
        levelMgr.StartNewLevel();
    }

    public void ContinueGame() 
    {
        levelMgr.RestartLevel();
    }

    public void NextLevel(string levelName)
    {
        levelMgr.StartLevel(levelName);
    }
}