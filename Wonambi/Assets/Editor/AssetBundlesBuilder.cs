using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using GlobalDefines;
using SimpleJSON;

public class AssetBundlesBuilder
{
    [MenuItem("Assets/Build AssetBundles And Generate Levels")]
    static void OldBuildAllAssetBundles()
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

    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAssetBundles()
    {
        // bundles分为三种
        // 1, player，和玩家相关的数据
        // 2，level，地图相关数据，包括地图上所有的元素
        // 3，config, 配置文件数据

        // 先根据png来生成level
        string levelDirectoryPath = "Assets/Bundles/Prefabs/Levels";
        string pngDirectoryPath = "Assets/Bundles/Sprites/Maps";

        // TODO 读取文件获取color和prefab名称的对应
        DirectoryInfo pngDirectory = new DirectoryInfo(pngDirectoryPath);
        if (!pngDirectory.Exists) {
            Debug.Log("[AssetBundlesBuilder] No PNG File Found! Path = " + pngDirectoryPath);
            return;
        }

        DirectoryInfo levelDirectory = new DirectoryInfo(levelDirectoryPath);
        if (!levelDirectory.Exists) {
            levelDirectory.Create();
        }

        // TODO 初始化hash表
        FileInfo[] levelFiles = levelDirectory.GetFiles();
        foreach (var item in levelFiles) {
            item.Delete();
        }
        var configBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + FilePath.ConfigBundlePath);
        Dictionary<string, string> colorToPrefab = new Dictionary<string, string>();

        TextAsset ctpFile = configBundle.LoadAsset<TextAsset>(FilePath.ColorToPrefabFile);
        if (ctpFile == null) {
            Debug.LogError("[AssetBundlesBuilder] ctpFile not found! Path = " + FilePath.ColorToPrefabFile);
            return;
        }
        colorToPrefab.Clear();
        JSONClass ctpJson = JSON.Parse(ctpFile.text) as JSONClass;
        JSONArray ctpArray = ctpJson["ColorToPrefab"].AsArray;
        for (int i = 0; i < ctpArray.Count; ++i) {
            JSONClass ctpData = ctpArray[i] as JSONClass;
            colorToPrefab[ctpData["color"]] = ctpData["prefab"];
        }
        Debug.Log("[AssetBundlesBuilder] json loaded. count = " + colorToPrefab.Count.ToString());

        foreach (var png in pngDirectory.GetFiles()) {
            GameObject level = new GameObject(Path.GetFileNameWithoutExtension(png.Name));
            BuildMapPrefab(level, png, colorToPrefab);
            Debug.Log("[AssetBundlesBuilder] CreatePrefab name = " + levelDirectoryPath + "/" + level.name + ".prefab");
            PrefabUtility.CreatePrefab(levelDirectoryPath + "/" + level.name + ".prefab", level);
        }

    }

    private static void BuildMapPrefab(GameObject level, FileInfo png, Dictionary<string, string> colorToPrefab)
    {
        /*
        byte[] fileData = File.ReadAllBytes(png.FullName);
        Texture2D pngMap = new Texture2D(2, 2);
        pngMap.LoadImage(fileData);
        Color32[] allPixels = pngMap.GetPixels32();
        int width = pngMap.width;
        int height = pngMap.height;
        Debug.Log("[BuildAllAssetBundles] pngMap w = " + width.ToString() + ", h = " + height.ToString() + ", name = " + png.Name);
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                SpawnObjectAt(allPixels, x, y, width, height, colorToPrefab);
            }
        }
        */
    }
    /*
    private static bool IsColorNotTile(Color32 c, Dictionary<string, string> colorToPrefab)
    {
        string colorKey = ColorUtility.ToHtmlStringRGBA(c);
        if (colorToPrefab.ContainsKey(colorKey) && colorToPrefab[colorKey] == "Tile") {
            return false;
        }
        return true;
    }

    private static bool IsDirectionTile(Color32 c, Dictionary<string, string> colorToPrefab)
    {
        string colorKey = ColorUtility.ToHtmlStringRGBA(c);
        if (colorToPrefab.ContainsKey(colorKey) && colorToPrefab[colorKey] == "Direction") {
            return true;
        }
        return false;
    }

    private static void SpawnObjectAt(Color32[] pixels, int x, int y, int width, int height, Dictionary<string, string> colorToPrefab)
    {
        Color32 c = pixels[(y * width) + x];
        if (c.a <= 0) return;
        string colorKey = ColorUtility.ToHtmlStringRGBA(c);
        if (LevelMgr.Instance.IsColorToPrefabKeyExists(colorKey)) {
            string prefab = LevelMgr.Instance.PrefabOfColor(colorKey);
            if (prefab == "Tile") {
                bool top = y >= height - 1 || IsColorNotTile(pixels[((y + 1) * width) + x], colorToPrefab);
                bool bottom = y <= 0 || IsColorNotTile(pixels[((y - 1) * width) + x],colorToPrefab);
                bool left = x <= 0 || IsColorNotTile(pixels[(y * width) + x - 1],colorToPrefab);
                bool right = x >= width - 1 || IsColorNotTile(pixels[(y * width) + x + 1],colorToPrefab);

                if (top && bottom && left && right) {
                    prefab += "Block";
                }
                else if (top && bottom && left && !right) {
                    prefab += "PlatformL";
                }
                else if (top && bottom && !left && right) {
                    prefab += "PlatformR";
                }
                else if (top && bottom && !left && !right) {
                    prefab += "PlatformM";
                }
                else if (top && !bottom && left && right) {
                    prefab += "PillarT";
                }
                else if (top && !bottom && left && !right) {
                    prefab += "GroundLT";
                }
                else if (top && !bottom && !left && right) {
                    prefab += "GroundRT";
                }
                else if (top && !bottom && !left && !right) {
                    prefab += "GroundT";
                }
                else if (!top && bottom && left && right) {
                    prefab += "PillarB";
                }
                else if (!top && bottom && left && !right) {
                    prefab += "GroundLB";
                }
                else if (!top && bottom && !left && right) {
                    prefab += "GroundRB";
                }
                else if (!top && bottom && !left && !right) {
                    prefab += "GroundB";
                }
                else if (!top && !bottom && left && right) {
                    prefab += "PillarM";
                }
                else if (!top && !bottom && left && !right) {
                    prefab += "GroundL";
                }
                else if (!top && !bottom && !left && right) {
                    prefab += "GroundR";
                }
                else if (!top && !bottom && !left && !right) {
                    return;
                }
                GameObject go = GameObject.Instantiate(BundleMgr.Instance.GetTile(prefab), new Vector3(x, y, 0), Quaternion.identity);
                go.transform.SetParent(transform);
            }
            else if (prefab == "PlayerSpawnPoint") {
                startPoint = new Vector2(x, y);
                return;
            }
            else if (prefab == "Escalator") {
                // 加一个中继点
                GameObject turnPoint = Instantiate(BundleMgr.Instance.GetTile("TurnPoint"), new Vector3(x, y, 0), Quaternion.identity);
                turnPoint.transform.SetParent(transform);

                // 根据周围地块来分析方向
                bool top = y < height - 1 && IsDirectionTile(pixels[((y + 1) * width) + x]);
                bool bottom = y > 0 && IsDirectionTile(pixels[((y - 1) * width) + x]);
                bool left = x > 0 && IsDirectionTile(pixels[(y * width) + x - 1]);
                bool right = x < width - 1 && IsDirectionTile(pixels[(y * width) + x + 1]);
                TurnPointController tpCtrl = turnPoint.GetComponent<TurnPointController>();
                if (top) {
                    if (tpCtrl.direction1 == MoveDirection.None) {
                        tpCtrl.direction1 = MoveDirection.Up;
                    }
                    else if (tpCtrl.direction2 == MoveDirection.None) {
                        tpCtrl.direction2 = MoveDirection.Up;
                    }
                }
                if (bottom) {
                    if (tpCtrl.direction1 == MoveDirection.None) {
                        tpCtrl.direction1 = MoveDirection.Down;
                    }
                    else if (tpCtrl.direction2 == MoveDirection.None) {
                        tpCtrl.direction2 = MoveDirection.Down;
                    }
                }
                if (left) {
                    if (tpCtrl.direction1 == MoveDirection.None) {
                        tpCtrl.direction1 = MoveDirection.Left;
                    }
                    else if (tpCtrl.direction2 == MoveDirection.None) {
                        tpCtrl.direction2 = MoveDirection.Left;
                    }
                }
                if (right) {
                    if (tpCtrl.direction1 == MoveDirection.None) {
                        tpCtrl.direction1 = MoveDirection.Right;
                    }
                    else if (tpCtrl.direction2 == MoveDirection.None) {
                        tpCtrl.direction2 = MoveDirection.Right;
                    }
                }
                // 加一个电梯
                GameObject eGo = Instantiate(BundleMgr.Instance.GetTile("MovingPlatform"), new Vector3(x, y, 0), Quaternion.identity);
                eGo.transform.SetParent(transform);
                eGo.GetComponent<MovingPlatformController>().auto = false;
            }
            else if (prefab == "TurnPoint") {
                // 加一个中继点
                GameObject tGo = Instantiate(BundleMgr.Instance.GetTile("TurnPoint"), new Vector3(x, y, 0), Quaternion.identity);
                tGo.transform.SetParent(transform);

                // 根据周围地块来分析方向
                bool top = IsDirectionTile(pixels[((y + 1) * width) + x]);
                bool bottom = IsDirectionTile(pixels[((y - 1) * width) + x]);
                bool left = IsDirectionTile(pixels[(y * width) + x - 1]);
                bool right = IsDirectionTile(pixels[(y * width) + x + 1]);
                TurnPointController tpCtrl = tGo.GetComponent<TurnPointController>();
                if (top) {
                    if (tpCtrl.direction1 == MoveDirection.None) {
                        tpCtrl.direction1 = MoveDirection.Up;
                    }
                    else if (tpCtrl.direction2 == MoveDirection.None) {
                        tpCtrl.direction2 = MoveDirection.Up;
                    }
                }
                if (bottom) {
                    if (tpCtrl.direction1 == MoveDirection.None) {
                        tpCtrl.direction1 = MoveDirection.Down;
                    }
                    else if (tpCtrl.direction2 == MoveDirection.None) {
                        tpCtrl.direction2 = MoveDirection.Down;
                    }
                }
                if (left) {
                    if (tpCtrl.direction1 == MoveDirection.None) {
                        tpCtrl.direction1 = MoveDirection.Left;
                    }
                    else if (tpCtrl.direction2 == MoveDirection.None) {
                        tpCtrl.direction2 = MoveDirection.Left;
                    }
                }
                if (right) {
                    if (tpCtrl.direction1 == MoveDirection.None) {
                        tpCtrl.direction1 = MoveDirection.Right;
                    }
                    else if (tpCtrl.direction2 == MoveDirection.None) {
                        tpCtrl.direction2 = MoveDirection.Right;
                    }
                }
            }
            else if (prefab == "Direction") {
                return;
            }
            else {
                GameObject go = Instantiate(BundleMgr.Instance.GetObject(prefab), new Vector3(x, y, 0), Quaternion.identity);
                go.transform.SetParent(transform);
            }
        }
        else {
            Debug.LogError("[LevelLoader] SpawnTileAt : no color to prefab found for: " + colorKey + ", " + c.ToString() + ", " + x + ", " + y);
        }
    }
    */
}