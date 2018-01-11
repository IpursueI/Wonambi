using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class GameDirector : MonoBehaviour {

    private BundleMgr bundleMgr;
    private LevelMgr levelMgr;
    private GameMgr gameMgr;
    private bool inGame;

    void Awake()
    {
        bundleMgr = BundleMgr.Instance;
        bundleMgr.Init();
        levelMgr = LevelMgr.Instance;
        levelMgr.Init();
        gameMgr = GameMgr.Instance;
        gameMgr.Init();
        // Hide cursor
        Cursor.visible = false;
    }

	// Use this for initialization
	void Start () {
        //LevelMgr.Instance.StartNewLevel();
        inGame = false;
        gameMgr.UIOnStart();
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void StartCutscene() {
        gameMgr.ShowCutscene();   
    }

    public void StartGame()
    {
        gameMgr.ShowGameUI();
        inGame = true;
        NewGame();
    }

    public void NewGame()
    {
        levelMgr.StartNewLevel();
    }

    public void ContinueGame() 
    {
       levelMgr.RestartLevel();
    }

    public bool IsInGame()
    {
        return inGame;
    }

    public void QuitGame()
    {
        inGame = false;
    }
}
