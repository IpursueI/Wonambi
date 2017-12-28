using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class GameDirector : MonoBehaviour {

    private BundleMgr bundleMgr;
    private LevelMgr levelMgr;
    private UIController uiController;
    private bool inGame;
    private AudioController audioController;

    void Awake()
    {
        bundleMgr = BundleMgr.Instance;
        bundleMgr.Init();
        levelMgr = LevelMgr.Instance;
        levelMgr.Init(this);
        uiController = GameObject.Find("UICanvas").GetComponent<UIController>();
        audioController = GameObject.Find("GameDirector").GetComponent<AudioController>();
        // Hide cursor
        Cursor.visible = false;
    }

	// Use this for initialization
	void Start () {
        //LevelMgr.Instance.StartNewLevel();
        inGame = false;
        uiController.ShowMenuUI();
        uiController.gameObject.SetActive(true);
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void StartCutscene() {
        uiController.ShowCutsceneUI();    
    }

    public void StartGame()
    {
        uiController.ShowGameUI();
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

    public void NextLevel(string levelName)
    {
        levelMgr.StartLevel(levelName);
    }

    public bool IsInGame()
    {
        return inGame;
    }

    public AudioController GetAudio()
    {
        return audioController;
    }
}
