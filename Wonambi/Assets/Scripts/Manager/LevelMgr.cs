﻿using System.Collections;
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
        SpawnPlayer();
    }

    public void StartLevel(string levelName)
    {
        LoadLevel(levelName);
        curLevel = levelName;
        TeleportPlayer();
    }

    public void RebornPlayer(Vector3 savePos)
    {
        Vector3 savePointVec3 = new Vector3(savePos.x, savePos.y, -10.0f);
        PlayerPrefs.SetString(PrefsKey.LevelMap, curLevel);
        PlayerPrefs.SetString(PrefsKey.SavePoint, savePointVec3.ToString());

        uiController.ShowBonfireMask();

        StartCoroutine(RebornCoroutine());
    }

    public IEnumerator RebornCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        RestartLevel();
        uiController.HideBonfireMask();
    }

    public void RestartLevel()
    {
        string levelName = PlayerPrefs.GetString(PrefsKey.LevelMap, curLevel);
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
        player.GetComponent<PlayerController>().Init(false, DefineNumber.DefaultBulletNumber);
        player.GetComponent<PlayerModel>().Init(DefineNumber.DefaultHP);
        player.transform.position = levelObj.GetComponent<LevelContext>().startPoint;
        player.transform.SetParent(null);

        // Save
        PlayerPrefs.SetInt(PrefsKey.PlayerMaxHP, player.GetComponent<PlayerModel>().GetMaxHP());
        PlayerPrefs.SetInt(PrefsKey.PlayerEnableDoubleJump, player.GetComponent<PlayerController>().GetDoubleJump() ? 1 : 0);
        PlayerPrefs.SetInt(PrefsKey.PlayerBulletNumber, player.GetComponent<PlayerController>().GetPlayerMaxBulletNumber());
        PlayerPrefs.SetString(PrefsKey.LevelMap, curLevel);
        PlayerPrefs.SetString(PrefsKey.SavePoint, levelObj.GetComponent<LevelContext>().startPoint.ToString());
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
        player.GetComponent<PlayerController>().Init(enableDoubleJump, bulletNumber);
        int maxHp = PlayerPrefs.GetInt(PrefsKey.PlayerMaxHP, DefineNumber.DefaultHP);
        player.GetComponent<PlayerModel>().Init(maxHp);
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
        } else {
            GameMgr.Instance.PlayBossBGM();
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

    private void ResetLevelPrefs()
    {
        PlayerPrefs.DeleteKey(PrefsKey.LevelDoubleJump);
        PlayerPrefs.DeleteKey(PrefsKey.LevelExtraBullet);
        PlayerPrefs.DeleteKey(PrefsKey.LevelBinaryDoor);
    }

    public void SaveDoubleJumpItem()
    {
        string doubleJumpItem = PlayerPrefs.GetString(PrefsKey.LevelDoubleJump, "");
        if(doubleJumpItem == "")
        {
            doubleJumpItem = curLevel;
        }
        else
        {
            doubleJumpItem += ("," + curLevel);
        }
        Debug.Log("[LevelMgr] SaveDoubleJumpItem, doubleJumpItem = " + doubleJumpItem);
        PlayerPrefs.SetString(PrefsKey.LevelDoubleJump, doubleJumpItem);
    }

    public void SaveExtraBulletItem()
    {
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

    public void SaveBinaryDoor()
    {
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

}