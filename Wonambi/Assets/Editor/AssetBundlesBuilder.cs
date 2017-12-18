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
        Debug.Log("[AssetBundlesBuilder] BuildAllAssetBundles path = " + assetBundleDirectoryPath);
#if UNITY_STANDALONE_OSX
        BuildPipeline.BuildAssetBundles(assetBundleDirectoryPath, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneOSXIntel);
#elif UNITY_STANDALONE_WIN
        BuildPipeline.BuildAssetBundles(assetBundleDirectoryPath, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneWindows64);
#endif
        Debug.Log("[AssetBundlesBuilder] BuildAllAssetBundles Done.");
    }
}