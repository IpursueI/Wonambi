using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class GameMgr : MonoBehaviour {

    private LevelLoader levelLoader;
    private GameObject player;

    private Vector3 playerSavePoint;
	// Use this for initialization
	void Start () 
    {
        PlayerPrefs.DeleteAll();
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
        LoadPlayPref();
        SpawnPlayer();

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
    }

    public Vector3 GetPlayerSpawnPos()
    {
        return playerSavePoint;
    }
}
