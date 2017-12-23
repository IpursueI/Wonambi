using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class GameDirector : MonoBehaviour {

    private BundleMgr bundleMgr;
    private LevelMgr levelMgr;
    private UIController uiController;
    void Awake()
    {
        bundleMgr.Instance.Init();
        levelMgr.Instance.Init();
        uiController = GameObject.Find("UICanvas").GetComponent<UIController>();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NewGame()
    {
        levelMgr.Instance.StartNewLevel();
    }

    public void ContinueGame() 
    {
        levelMgr.Instance.RestartLevel();
    }

    public void NextLevel(string levelName)
    {
        levelMgr.Instance.StartLevel(levelName);
    }
}
