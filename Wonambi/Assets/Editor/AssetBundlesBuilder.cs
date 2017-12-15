using UnityEditor;
using UnityEngine;
using System.IO;

public class AssetBundlesBuilder
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectoryPath = Application.streamingAssetsPath;
        DirectoryInfo assetBundleDirectory = new DirectoryInfo(assetBundleDirectoryPath);
        if(!assetBundleDirectory.Exists) {
            assetBundleDirectory.Create();
        }
        FileInfo[] files = assetBundleDirectory.GetFiles();
        foreach(var item in files) {
            item.Delete();
        }
#if UNITY_STANDALONE_OSX
        BuildPipeline.BuildAssetBundles(assetBundleDirectoryPath, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneOSXIntel);
#endif
    }
}