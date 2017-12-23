using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class GameMgr : MonoBehaviour {

    private LevelLoader levelLoader;
    private GameObject player;

    private Vector3 playerSavePoint;
    private UIController uiCtrl;
    private string curLevel;
	// Use this for initialization
	void Start () 
    {
        PlayerPrefs.DeleteAll();
        BundleMgr.Instance.Init();
        levelLoader = GameObject.Find("LevelContainer").GetComponent<LevelLoader>();
        uiCtrl = GameObject.Find("UICanvas").GetComponent<UIController>();
        uiCtrl.gameObject.SetActive(false);

        curLevel = "levelMap1";
        // TODO test
        StartLevel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartLevel() 
    {
        levelLoader.LoadLevel(curLevel);
        LoadPlayPref();
        SpawnPlayer();
        uiCtrl.gameObject.SetActive(true);
    }

    void LoadPlayPref()
    {
        string spStr = PlayerPrefs.GetString(PrefsKey.PlayerSavePoint, "");
        if(spStr == "") {
            playerSavePoint = new Vector3(levelLoader.startPoint.x, levelLoader.startPoint.y, -10);
        } else {
            playerSavePoint = GlobalFunc.StringToVector3(spStr);
        }
    }

    void SpawnPlayer() 
    {
        if(player == null) {
            player = Instantiate(BundleMgr.Instance.GetObject("Player"), 
                                 playerSavePoint,
                                 Quaternion.identity);
            player.GetComponent<PlayerWithRigidBodyController>().Init();
            player.GetComponent<PlayerModel>().Init();
            uiCtrl.ShowHp(player.GetComponent<PlayerModel>().hp);
        } else {
            player.transform.position = playerSavePoint;
            player.GetComponent<PlayerModel>().Init();
            player.GetComponent<PlayerWithRigidBodyController>().Init();
            uiCtrl.ShowHp(player.GetComponent<PlayerModel>().hp);
        }
    }

    public bool IsPlayerClose(Vector3 monsterPos) 
    {
        if (player == null) return false;
        Vector3 pPos = player.transform.position;
        float distancePow = Mathf.Pow(monsterPos.x - pPos.x, 2) + Mathf.Pow(monsterPos.y - pPos.y, 2);
       // Debug.Log("[IsPlayerClose] pPos = " + pPos.ToString() + ", mPos = " + monsterPos.ToString() + ", distance = " + distancePow.ToString());
        if(distancePow < DefineNumber.MonsterMoveTriggerDistance * DefineNumber.MonsterMoveTriggerDistance) {
            return true;
        }
        return false;
    }

    public bool IsPlayerTrigger(Vector3 monsterPos) {
        if (player == null) return false;
        Vector3 pPos = player.transform.position;
        float distancePow = Mathf.Pow(monsterPos.x - pPos.x, 2) + Mathf.Pow(monsterPos.y - pPos.y, 2);
        if(distancePow < DefineNumber.MonsterFireTriggerDistance * DefineNumber.MonsterFireTriggerDistance) {
            return true;
        }
        return false;
    }

    public bool IsPlayerRight(Vector3 monsterPos) {
        if (player == null) return false;
        Vector3 pPos = player.transform.position;
        if (pPos.x < monsterPos.x) return false;
        return true;
    }

    public float DistanceToPlayer(Vector3 monsterPos) {
        if (player == null) return -1.0f;
        Vector3 pPos = player.transform.position;
        return Mathf.Pow(monsterPos.x - pPos.x, 2) + Mathf.Pow(monsterPos.y - pPos.y, 2);
    }

    public void SetSavePoint(Vector3 savePos) 
    {
        playerSavePoint = new Vector3(savePos.x, savePos.y, -10);
        PlayerPrefs.SetString(PrefsKey.PlayerSavePoint, playerSavePoint.ToString());
        if(player != null) {
            player.GetComponent<PlayerModel>().FullHP();
        }
    }

    public Vector3 GetPlayerSpawnPos()
    {
        return playerSavePoint;
    }

    public void RefreshHP()
    {
        if (player == null) return;
        uiCtrl.ShowHp(player.GetComponent<PlayerModel>().hp);
    }
}
