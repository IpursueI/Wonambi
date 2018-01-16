using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;
using SimpleJSON;

public class LevelMgr : Singleton<LevelMgr>
{
    protected LevelMgr() { }
    public string identify = "LevelMgr";

    private GameObject levelContainer;
    private GameObject levelObj;
    private GameObject player;

    private Dictionary<string, JSONClass> levelConfigHash = new Dictionary<string, JSONClass>();

    private string curLevel;
    private CameraController cameraController;
    private UIController uiController;

    public void Init()
    {
        levelContainer = GameObject.Find("LevelContainer");
        uiController = GameObject.Find("UICanvas").GetComponent<UIController>();
        cameraController = GameObject.Find("MainCamera").GetComponent<CameraController>();


        TextAsset levelFile = BundleMgr.Instance.GetJson("LevelConfig");
        if (levelFile == null) {
            Debug.LogError("[ThemeMgr] ThemeFile not found!");
            return;
        }
        levelConfigHash.Clear();
        JSONClass levelJson = JSON.Parse(levelFile.text) as JSONClass;
        JSONArray levelArray = levelJson["LevelConfig"].AsArray;
        for (int i = 0; i < levelArray.Count; ++i) {
            JSONClass levelData = levelArray[i] as JSONClass;
            string levelName = levelData["Name"];
            levelConfigHash[levelName] = levelData;
            Debug.Log("[LevelMgr] Init levelName = " + levelName);
        }
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
        ResetLevelPrefs();
        curLevel = DefineString.FirstLevel;
        var playerPos = LoadLevel(curLevel);
        PlayerPrefs.SetString(PrefsKey.LevelMap, curLevel);
        SpawnPlayer(playerPos);
    }

    public void NextLevel()
    {
        if (levelObj == null) return;
        string levelName = levelObj.GetComponent<LevelContext>().nextLevel;
        StartLevel(levelName);
    }

    public void StartLevel(string levelName)
    {
        curLevel = levelName;
        PlayerPrefs.SetString(PrefsKey.LevelMap, curLevel);
        var playerPos = LoadLevel(curLevel);
        TeleportPlayer(playerPos);
    }

    public void RestartLevel()
    {
        string levelName = PlayerPrefs.GetString(PrefsKey.LevelMap, curLevel);
        curLevel = levelName;
        var playerPos = LoadLevel(curLevel);
        LoadPlayer(playerPos);
    }

    private void SpawnPlayer(Vector3 startPos)
    {
        if(player == null) {
            player = Instantiate(BundleMgr.Instance.GetObject("Player"), Vector3.zero, Quaternion.identity);
            cameraController.SetPlayer(player);
            GameMgr.Instance.EnableInput();
        }
        player.GetComponent<PlayerController>().Init(false, DefineNumber.DefaultBulletNumber, DefineNumber.DefaultHP);
        player.transform.position = startPos;
        player.transform.SetParent(null);
    }

    private void TeleportPlayer(Vector3 pos)
    {
        if (player == null) {
            Debug.LogError("[LevelMgr] TeleportPlayer failed. player is null.");
            return;
        }

        player.transform.position = pos;
    }

    private void LoadPlayer(Vector3 startPos)
    {
        if(player == null) {
            player = Instantiate(BundleMgr.Instance.GetObject("Player"), Vector3.zero, Quaternion.identity);
            cameraController.SetPlayer(player);
            GameMgr.Instance.EnableInput();
        }
        bool enableDoubleJump = PlayerPrefs.GetInt(PrefsKey.PlayerEnableDoubleJump, 0) > 0;
        int bulletNumber = PlayerPrefs.GetInt(PrefsKey.PlayerBulletNumber, DefineNumber.DefaultBulletNumber);
        int maxHp = PlayerPrefs.GetInt(PrefsKey.PlayerMaxHP, DefineNumber.DefaultHP);
        player.GetComponent<PlayerController>().Init(enableDoubleJump, bulletNumber, maxHp);
        player.transform.position = startPos;
        player.transform.SetParent(null);
    }

    private Vector3 LoadLevel(string levelName)
    {
        ClearLevel();
        levelObj = Instantiate(BundleMgr.Instance.GetLevel(levelName), Vector3.zero, Quaternion.identity);
        levelObj.transform.SetParent(levelContainer.transform);
        LevelContext levelContext = levelObj.GetComponent<LevelContext>();
        cameraController.SetScreenSize(levelContext.width, levelContext.height);
        var levelData = levelConfigHash[levelName];
        levelContext.bgm = levelData["BGM"];
        levelContext.nextLevel = levelData["NextLevel"];
        levelContext.cameraSize = levelData["CameraSize"].AsFloat;

        cameraController.SetOrthoSize(levelContext.cameraSize);
        GameMgr.Instance.PlayBGM(levelContext.bgm);
        // DoubleJump
        string doubleJumpItem = PlayerPrefs.GetString(PrefsKey.LevelDoubleJump, "");
        string[] doubleJumpArray = doubleJumpItem.Split(','); 
        foreach(string item in doubleJumpArray)
        {
            if(levelContext.levelName == item && levelContext.doubleJumpItem != null)
            {
                levelContext.doubleJumpItem.SetActive(false);
            }
        }

        string extraBulletItem = PlayerPrefs.GetString(PrefsKey.LevelExtraBullet, "");
        string[] extraBulletArray = extraBulletItem.Split(',');
        foreach (string item in extraBulletArray)
        {
            if (levelContext.levelName == item && levelContext.extraBulletItem != null)
            {
                levelContext.extraBulletItem.SetActive(false);
            }
        }

        return levelContext.startPoint;
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
        float distanceX = Mathf.Abs(monsterPos.x - pPos.x);
        float distanceY = Mathf.Abs(monsterPos.y - pPos.y);
        if(distanceX < DefineNumber.MonsterMoveTriggerDistanceX && distanceY < DefineNumber.MonsterMoveTriggerDistanceY)
        {
            return true;
        }
        return false;
    }

    public bool IsPlayerSoClose(Vector3 monsterPos)
    {
        if (player == null) return false;
        Vector3 pPos = player.transform.position;
        float distanceX = Mathf.Abs(monsterPos.x - pPos.x);
        float distanceY = Mathf.Abs(monsterPos.y - pPos.y);
        if (distanceX < DefineNumber.MonsterFireTriggerDistanceX && distanceY < DefineNumber.MonsterFireTriggerDistanceY)
        {
            return true;
        }
        return false;
    }

    public void RefreshHP()
    {
        if(player != null) {
            int hp = player.GetComponent<PlayerModel>().hp;
            uiController.ShowHp(hp);
        }
    }

    private void ResetLevelPrefs()
    {
        PlayerPrefs.DeleteKey(PrefsKey.LevelDoubleJump);
        PlayerPrefs.DeleteKey(PrefsKey.LevelExtraBullet);
        PlayerPrefs.DeleteKey(PrefsKey.LevelBinaryDoor);
    }

    private void SaveDoubleJumpItem()
    {
        if (levelObj == null ||
            levelObj.GetComponent<LevelContext>().doubleJumpItem == null ||
            levelObj.GetComponent<LevelContext>().isDoubleJumpDone == false)
        {
            return;
        }
        string doubleJumpItem = PlayerPrefs.GetString(PrefsKey.LevelDoubleJump, "");
        if(doubleJumpItem == "")
        {
            doubleJumpItem = curLevel;
        }
        else
        {
            doubleJumpItem += ("," + curLevel);
        }
        PlayerPrefs.SetString(PrefsKey.LevelDoubleJump, doubleJumpItem);
    }

    private void SaveExtraBulletItem()
    {
        if (levelObj == null ||
            levelObj.GetComponent<LevelContext>().extraBulletItem == null ||
            levelObj.GetComponent<LevelContext>().isExtraBulletDone == false)
        {
            return;
        }
        string extraBulletItem = PlayerPrefs.GetString(PrefsKey.LevelExtraBullet, "");
        if (extraBulletItem == "")
        {
            extraBulletItem = curLevel;
        }
        else
        {
            extraBulletItem += ("," + curLevel);
        }
        PlayerPrefs.SetString(PrefsKey.LevelExtraBullet, extraBulletItem);
    }

    public void OnEnableDoubleJump()
    {
        if(levelObj)
        {
            LevelContext context = levelObj.GetComponent<LevelContext>();
            if(context.doubleJumpItem != null)
            {
                context.isDoubleJumpDone = true;
                SaveDoubleJumpItem();
            }
        }
    }

    public void OnEnableExtraBullet()
    {
        if (levelObj)
        {
            LevelContext context = levelObj.GetComponent<LevelContext>();
            if (context.extraBulletItem != null)
            {
                context.isExtraBulletDone = true;
                SaveExtraBulletItem();
            }
        }
    }

    public void OnEnableBinaryDoor()
    {
        if (levelObj)
        {
            LevelContext context = levelObj.GetComponent<LevelContext>();
            if (context.binaryDoorItem != null)
            {
                context.isBinaryDoorDown = true;
            }
        }
    }

    public Vector3 GetPlayerPos()
    {
        if (player == null) return Vector3.zero;
        return player.transform.position;
    }

    public void QuitGame()
    {
        if(levelObj != null) {
            Destroy(levelObj);
        }
        if(player != null) {
            Destroy(player);
        }
    }

    public void ShowTips(string content, float duration)
    {
        if(player == null) {
            return;
        }   
        player.GetComponent<PlayerDialogController>().ShowDialog(content, duration);
    }

    public void HideTips()
    {
        if(player == null) {
            return;
        }
        player.GetComponent<PlayerDialogController>().HideDialog();
    }
}