using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class LevelMgr : Singleton<LevelMgr>
{
    protected LevelMgr() { }
    public string identify = "LevelMgr";

    private GameObject levelContainer;
    private GameObject levelObj;
    private GameObject player;

    private string curLevel;
    private GameDirector gameDirector;
    private CameraController cameraController;
    private UIController uiController;

    public void Init(GameDirector director)
    {
        levelContainer = GameObject.Find("LevelContainer");
        uiController = GameObject.Find("UICanvas").GetComponent<UIController>();
        cameraController = GameObject.Find("MainCamera").GetComponent<CameraController>();
        gameDirector = director;
    }

    public void ClearLevel()
    {
        if(levelObj != null) {
            if(player != null) player.transform.SetParent(null);
            Destroy(levelObj);
            levelObj = null;
        }
    }

    public void StartNewLevel()
    {
        LoadLevel(DefineString.FirstLevel);
        curLevel = DefineString.FirstLevel;
        SpawnPlayer();
    }

    public void StartLevel(string levelName)
    {
        LoadLevel(levelName);
        curLevel = levelName;
        TeleportPlayer();
    }

    public void RestartLevel()
    {
        LoadLevel(curLevel);
        LoadPlayer();
    }

    public void RebornPlayer(Vector3 savePos)
    {
        Vector3 savePointVec3 = new Vector3(savePos.x, savePos.y, -10.0f);
        PlayerPrefs.SetString(PrefsKey.LevelMap, curLevel);
        PlayerPrefs.SetString(PrefsKey.SavePoint, savePointVec3.ToString());
        RestartLevel();
    }

    private void SpawnPlayer()
    {
        if(player == null) {
            player = Instantiate(BundleMgr.Instance.GetObject("Player"), Vector3.zero, Quaternion.identity);
            cameraController.SetPlayer(player);
        }

        player.GetComponent<PlayerController>().Init(false);
        player.GetComponent<PlayerModel>().Init(DefineNumber.DefaultHP);
        player.transform.position = levelObj.GetComponent<LevelController>().startPoint;
        player.transform.SetParent(null);

        // Save
        PlayerPrefs.SetInt(PrefsKey.PlayerMaxHP, player.GetComponent<PlayerModel>().GetMaxHP());
        PlayerPrefs.SetInt(PrefsKey.PlayerMaxHP, player.GetComponent<PlayerController>().GetDoubleJump() ? 1 : 0);
        PlayerPrefs.SetString(PrefsKey.LevelMap, curLevel);
        PlayerPrefs.SetString(PrefsKey.SavePoint, levelObj.GetComponent<LevelController>().startPoint.ToString());
    }

    private void TeleportPlayer()
    {
        if (player == null) {
            Debug.LogError("[LevelMgr] TeleportPlayer failed. player is null.");
            return;
        }

        player.transform.position = levelObj.GetComponent<LevelController>().startPoint;
    }

    private void LoadPlayer()
    {
        if(player == null) {
            player = Instantiate(BundleMgr.Instance.GetObject("Player"), Vector3.zero, Quaternion.identity);
            cameraController.SetPlayer(player);
        }

        player.GetComponent<PlayerController>().Init(false);
        player.GetComponent<PlayerModel>().Init(DefineNumber.DefaultHP);
        string posStr = PlayerPrefs.GetString(PrefsKey.SavePoint, "");
        if(posStr == "") {
            Debug.Log("[LevelMgr] LoadPlayer savepoint not found.");
            return;
        }
        player.transform.position = GlobalFunc.StringToVector3(posStr);
        player.transform.SetParent(null);
    }

    private void LoadLevel(string levelName)
    {
        ClearLevel();
        levelObj = Instantiate(BundleMgr.Instance.GetLevel(levelName), Vector3.zero, Quaternion.identity);
        levelObj.transform.SetParent(levelContainer.transform);
        LevelController levelController = levelObj.GetComponent<LevelController>();
        cameraController.SetScreenSize(levelController.width, levelController.height);
    }

    public bool IsPlayerRight(Vector3 monsterPos)
    {
        if (player == null) return false;
        Vector3 pPos = player.transform.position;
        if (pPos.x < monsterPos.x) return false;
        return true;
    }

    public float DistanceToPlayer(Vector3 monsterPos)
    {
        if (player == null) return -1.0f;
        Vector3 pPos = player.transform.position;
        return Mathf.Pow(monsterPos.x - pPos.x, 2) + Mathf.Pow(monsterPos.y - pPos.y, 2);
    }

    public bool IsPlayerClose(Vector3 monsterPos)
    {
        if (player == null) return false;
        Vector3 pPos = player.transform.position;
        float distancePow = Mathf.Pow(monsterPos.x - pPos.x, 2) + Mathf.Pow(monsterPos.y - pPos.y, 2);
        return distancePow < DefineNumber.MonsterMoveTriggerDistance * DefineNumber.MonsterMoveTriggerDistance;
    }

    public void RefreshHP()
    {
        if(player != null) {
            int hp = player.GetComponent<PlayerModel>().hp;
            uiController.ShowHp(hp);
        }
    }

}