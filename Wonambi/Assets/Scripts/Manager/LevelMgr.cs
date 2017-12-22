using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;
using SimpleJSON;

public class LevelMgr : Singleton<LevelMgr>
{
    protected LevelMgr() { }
    public string identify = "LevelMgr";

    private Dictionary<string, string> colorToPrefab = new Dictionary<string, string>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init()
    {
        TextAsset ctpFile = BundleMgr.Instance.GetJson(FilePath.ColorToPrefabFile);
        if (ctpFile == null) {
            Debug.LogError("[LevelMgr] ctpFile not found! Path = " + FilePath.ColorToPrefabFile);
            return;
        }
        colorToPrefab.Clear();
        JSONClass ctpJson = JSON.Parse(ctpFile.text) as JSONClass;
        JSONArray ctpArray = ctpJson["ColorToPrefab"].AsArray;
        for (int i = 0; i < ctpArray.Count; ++i) {
            JSONClass ctpData = ctpArray[i] as JSONClass;
            colorToPrefab[ctpData["color"]] = ctpData["prefab"];
        }
    }

    public bool IsColorToPrefabKeyExists(string key)
    {
        return colorToPrefab.ContainsKey(key);
    }

    public string PrefabOfColor(string key)
    {
        if(!colorToPrefab.ContainsKey(key)) {
            return "";
        }
        return colorToPrefab[key];
    }
}
