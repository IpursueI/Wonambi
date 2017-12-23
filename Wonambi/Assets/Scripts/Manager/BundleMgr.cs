using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class BundleMgr : Singleton<BundleMgr>
{
    protected BundleMgr() { }
    public string identify = "BundleMgr";

    private AssetBundle levelBundle;
    private AssetBundle objectBundle;
    private AssetBundle configBundle;

    public void Init()
    {
        levelBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + FilePath.LevelBundlePath);
        objectBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + FilePath.ObjectBundlePath);
       // configBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + FilePath.ConfigBundlePath);
    }


    public GameObject GetLevel(string name)
    {
        try {
            return levelBundle.LoadAsset<GameObject>(name);
        } catch (NullReferenceException e) {
            Debug.LogError("[BundleMgr] GetLevel : " + e.ToString() + ", name = " + name);
            return null;
        }
    }

    public GameObject GetObject(string name)
    {
        try {
            return objectBundle.LoadAsset<GameObject>(name);
        } catch (NullReferenceException e) {
            Debug.LogError("[BundleMgr] GetObject : " + e.ToString() + ", name = " + name);
            return null;
        }
    }

    public TextAsset GetJson(string name)
    {
        try {
            return configBundle.LoadAsset<TextAsset>(name);
        }
        catch (NullReferenceException e) {
            Debug.LogError("[BundleMgr] GetJson : " + e.ToString() + ", name = " + name);
            return null;
        }
    }
}
