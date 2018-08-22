using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;
using SimpleJSON;

public class LevelMgr : Singleton<LevelMgr>
{
    protected LevelMgr() { }
    public string identify = "LevelMgr";

    private Dictionary<string, JSONClass> levelConfigHash = new Dictionary<string, JSONClass>();

    private GameObject levelContainer;
    private GameObject levelObj;
    private string savedLevel;

    public void Init()
    {
        levelContainer = GameObject.Find("LevelContainer");
        TextAsset levelFile = BundleMgr.Instance.GetJson("LevelConfig");
        if (levelFile == null) {
            Debug.LogError("[LeveMgr] LevelConfig not found!");
            return;
        }
        levelConfigHash.Clear();
        JSONClass levelJson = JSON.Parse(levelFile.text) as JSONClass;
        JSONArray levelArray = levelJson["LevelConfig"].AsArray;
        for (int i = 0; i < levelArray.Count; ++i) {
            JSONClass levelData = levelArray[i] as JSONClass;
            string levelName = levelData["Name"];
            levelConfigHash[levelName] = levelData;
        }
        savedLevel = PlayerPrefs.GetString(PrefsKey.SavedLevel, DefineString.FirstLevel);
    }

    public void ClearLevel()
    {
        if(levelObj != null) {
            if(player != null) player.transform.SetParent(null);
            Destroy(levelObj);
            levelObj = null;
        }
    }

    private Vector3 LoadLevel(string levelName)
    {
        ClearLevel();
        var levelData = levelConfigHash[levelName];
        levelObj = Instantiate(BundleMgr.Instance.GetLevel(levelName), Vector3.zero, Quaternion.identity);
        levelObj.transform.SetParent(levelContainer.transform);
        LevelContext levelContext = levelObj.GetComponent<LevelContext>();
        levelContext.Init(levelData);
    }

    // Start level first time
    public void StartNewLevel()
    {
        ResetLevelPrefs();
        LoadLevel(DefineString.FirstLevel);
        levelObj.GetComponent<LevelContext>().SpawnPlayer();
    }

    public void NextLevel()
    {
        if (levelObj == null) return;
        LevelContext curLevelContext = levelObj.GetComponent<LevelContext>();
        LoadLevel(curLevelContext.nextLevel);
        if(curLevelContext.isCheckPoint) {
            savedLevel = curLevelContext.levelName;
        }
        PlayerPrefs.SetString(PrefsKey.SavedLevel, savedLevel);
        levelContext.TeleportPlayer();
    }

    public void RestartLevel()
    {
        LoadLevel(savedLevel);
        SpawnPlayer();
    }

   

    private void ResetLevelPrefs()
    {
        // TODO 重制所有的现实和不现实的id
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
}