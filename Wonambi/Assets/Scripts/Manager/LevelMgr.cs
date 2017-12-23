using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class LevelMgr : Singleton<LevelMgr>
{
    protected LevelMgr() { }
    public string identify = "LevelMgr";

    private string curLevel;
    private string savePoint;

    private GameObject levelContainer;

    public void Init()
    {
       levelController = GameObject.Find("LevelContainer");
        savePoint = PlayerPrefs.GetString(PrefsKey.SavePoint, "");
        curLevel = PlayerPrefs.GetString(PrefsKey.LevelMap, "");
    }

    public void ClearLevel()
    {
        while(levelController.transform.childCount > 0) {
            Transform c = levelController.transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }
    }

    public void StartNewLevel()
    {
        LoadLevel(DefineString.FirstLevel);
    }

    public void StartLevel(string levelName)
    {
        LoadLevel(levelName);
    }

    public void RestartLevel()
    {
        ReLoadLevel();
    }

    public void LoadLevel(string levelName)
    {
        ClearLevel();
        GameObject levelGo = Instantiate(BundleMgr.Instance.GetLevel(levelName), Vector3.zero, Quaternion.identify);
        levelGo.transform.SetParent(levelContainer.transform);

        Vector2 savePointVec2 = levelGo.GetComponent<LevelController>().SavePoint;
        Vector3 savePointVec3 = new Vector3(savePointVec2.x, savePointVec2.y, -10.0f);

        savePoint = savePointVec3.ToString();
        curLevel = levelName;

        PlayerPrefs.SetString(PrefsKey.SavePoint, savePoint);
        PlayerPrefs.SetString(PrefsKey.LevelMap, curLevel);
    }

    public void ReLoadLevel()
    {
        if(savePoint == "" || curLevel == "") {
            Debug.Log("[LevelMgr] RestartLevel savePoint or levelName not found.");
            return;
        }

        GameObject levelGo = Instantiate(BundleMgr.Instance.GetLevel(curLevel), Vector3.zero, Quaternion.identify);
        levelGo.transform.SetParent(levelContainer.transform);
    }

    public void SetSavePoint(Vector3 savePos)
    {
        Vector3 savePointVec3 = new Vector3(savePos.x, savePos.y, -10.0f);
        savePoint = savePointVec3.ToString();

        PlayerPrefs.SetString(PrefsKey.SavePoint, savePoint);
        PlayerPrefs.SetString(PrefsKey.LevelMap, curLevel);
    }

    public string GetSavePoint()
    {
        return savePoint;
    }

}