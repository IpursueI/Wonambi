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
    private CameraController cameraController;
    private UIController uiController;

    public void Init()
    {
        levelContainer = GameObject.Find("LevelContainer");
        uiController = GameObject.Find("UICanvas").GetComponent<UIController>();
        cameraController = GameObject.Find("MainCamera").GetComponent<CameraController>();
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
        LoadLevel(DefineString.FirstLevel);
        curLevel = DefineString.FirstLevel;
        SaveCheckPoint(levelObj.GetComponent<LevelContext>().startPoint);
        SpawnPlayer();
    }

    public void StartLevel(string levelName)
    {
        LoadLevel(levelName);
        curLevel = levelName;
        TeleportPlayer();
    }

    public void OnTriggerSave(Vector3 savePos)
    {
        Vector3 savePoint = new Vector3(savePos.x, savePos.y, -10.0f);
        SaveCheckPoint(savePoint);
        SaveDoubleJumpItem();
        SaveExtraBulletItem();
        SaveBinaryDoor();
        if(player != null) {
            player.GetComponent<PlayerController>().OnTriggerSave();
        }
    }

    public void RestartLevel()
    {
        string levelName = PlayerPrefs.GetString(PrefsKey.LevelMap, curLevel);
        curLevel = levelName;
        LoadLevel(levelName);
        LoadPlayer();
    }

    private void SpawnPlayer()
    {
        if(player == null) {
            player = Instantiate(BundleMgr.Instance.GetObject("Player"), Vector3.zero, Quaternion.identity);
            cameraController.SetPlayer(player);
            GameMgr.Instance.EnableInput();
        }
        player.GetComponent<PlayerController>().Init(false, DefineNumber.DefaultBulletNumber, DefineNumber.DefaultHP);
        player.transform.position = levelObj.GetComponent<LevelContext>().startPoint;
        player.transform.SetParent(null);
    }

    private void TeleportPlayer()
    {
        if (player == null) {
            Debug.LogError("[LevelMgr] TeleportPlayer failed. player is null.");
            return;
        }

        player.transform.position = levelObj.GetComponent<LevelContext>().startPoint;
    }

    private void LoadPlayer()
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
        string posStr = PlayerPrefs.GetString(PrefsKey.SavePoint, "");
        if(posStr == "") {
            Debug.LogError("[LevelMgr] LoadPlayer savepoint not found.");
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
        LevelContext levelController = levelObj.GetComponent<LevelContext>();
        cameraController.SetScreenSize(levelController.width, levelController.height);
        if(levelName.Substring(0,5) == "Level") {
            GameMgr.Instance.PlayBGM();
            cameraController.SetOrthoSize(6.0f);
        } else {
            GameMgr.Instance.PlayBossBGM();
            cameraController.SetOrthoSize(8.0f);
        }

        // DoubleJump
        string doubleJumpItem = PlayerPrefs.GetString(PrefsKey.LevelDoubleJump, "");
        string[] doubleJumpArray = doubleJumpItem.Split(','); 
        foreach(string item in doubleJumpArray)
        {
            if(levelController.levelName == item && levelController.doubleJumpItem != null)
            {
                levelController.doubleJumpItem.SetActive(false);
            }
        }

        string extraBulletItem = PlayerPrefs.GetString(PrefsKey.LevelExtraBullet, "");
        string[] extraBulletArray = extraBulletItem.Split(',');
        foreach (string item in extraBulletArray)
        {
            if (levelController.levelName == item && levelController.extraBulletItem != null)
            {
                levelController.extraBulletItem.SetActive(false);
            }
        }

        string binaryDoorItem = PlayerPrefs.GetString(PrefsKey.LevelBinaryDoor, "");
        string[] binaryDoorArray = binaryDoorItem.Split(',');
        foreach (string item in binaryDoorArray)
        {
            if (levelController.levelName == item && levelController.binaryDoorItem != null)
            {
                levelController.binaryDoorItem.GetComponent<BinaryDoorController>().SetRightTrigger();
            }
        }
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

    private void SaveBinaryDoor()
    {
        if (levelObj == null ||
            levelObj.GetComponent<LevelContext>().binaryDoorItem == null ||
            levelObj.GetComponent<LevelContext>().isBinaryDoorDown == false)
        {
            return;
        }
        string binaryDoorItem = PlayerPrefs.GetString(PrefsKey.LevelBinaryDoor, "");
        if (binaryDoorItem == "")
        {
            binaryDoorItem = curLevel;
        }
        else
        {
            binaryDoorItem += ("," + curLevel);
        }
        PlayerPrefs.SetString(PrefsKey.LevelBinaryDoor, binaryDoorItem);
    }

    public void OnEnableDoubleJump()
    {
        if(levelObj)
        {
            LevelContext context = levelObj.GetComponent<LevelContext>();
            if(context.doubleJumpItem != null)
            {
                context.isDoubleJumpDone = true;
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

    private void SaveCheckPoint(Vector3 savePoint)
    {
        if(levelObj != null) {
            PlayerPrefs.SetString(PrefsKey.LevelMap, curLevel);
            PlayerPrefs.SetString(PrefsKey.SavePoint, savePoint.ToString());
        }
    }
}