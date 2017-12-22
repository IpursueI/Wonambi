using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class BundleMgr : Singleton<BundleMgr>
{
    protected BundleMgr() { }
    public string identify = "BundleMgr";

    private AssetBundle tileBundle;
    private AssetBundle levelBundle;
    private AssetBundle objectBundle;
    private AssetBundle configBundle;

    public void Init()
    {
        tileBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + FilePath.TileBundlePath);
        levelBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + FilePath.LevelBundlePath);
        objectBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + FilePath.ObjectBundlePath);
        configBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + FilePath.ConfigBundlePath);
    }

    public GameObject GetTile(string name)
    {
        try {
            return tileBundle.LoadAsset<GameObject>(name);
        } catch (NullReferenceException e) {
            Debug.LogError("[BundleMgr] GetTile : " + e.ToString());
            return null;
        }
    }

    public Texture2D GetLevelMap(string name)
    {
        try {
            return levelBundle.LoadAsset<Texture2D>(name);
        } catch (NullReferenceException e) {
            Debug.LogError("[BundleMgr] GetLevelMap : " + e.ToString());
            return null;
        }
    }

    public GameObject GetObject(string name)
    {
        try {
            return objectBundle.LoadAsset<GameObject>(name);
        } catch (NullReferenceException e) {
            Debug.LogError("[BundleMgr] GetObject : " + e.ToString());
            return null;
        }
    }

    public TextAsset GetJson(string name)
    {
        try {
            return  
        }
        catch (NullReferenceException e) {
            Debug.LogError("[BundleMgr] GetJson : " + e.ToString());
            return null;
        }
    }
}
