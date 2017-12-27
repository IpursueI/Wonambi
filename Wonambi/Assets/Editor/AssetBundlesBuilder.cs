using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using GlobalDefines;

public class AssetBundlesBuilder
{
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

        DirectoryInfo levelDirectory = new DirectoryInfo(levelDirectoryPath);
        if (!levelDirectory.Exists) {
            levelDirectory.Create();
        }
        foreach (string d in Directory.GetFileSystemEntries(levelDirectoryPath)) {
            if (File.Exists(d)) {
                File.Delete(d);
            }
        }
        DirectoryInfo pngDirectory = new DirectoryInfo(pngDirectoryPath);
        if (!pngDirectory.Exists) {
            Debug.Log("[AssetBundlesBuilder] No PNG File Found! Path = " + pngDirectoryPath);
            return;
        }

        // 初始化hash表
        Dictionary<string, string> colorToPrefab = new Dictionary<string, string>();

        colorToPrefab["000000FF"] = "Tile";
        colorToPrefab["89C997FF"] = "PlayerSpawnPoint";
        colorToPrefab["FF0000FF"] = "Pile";
        colorToPrefab["FF0001FF"] = "Sentry";
        colorToPrefab["FF0002FF"] = "Patrol";
        colorToPrefab["FF0003FF"] = "Fort";
        colorToPrefab["FF0004FF"] = "LurkerUp";
        colorToPrefab["FF0005FF"] = "LurkerDown";
        colorToPrefab["FF0006FF"] = "LurkerLeft";
        colorToPrefab["FF0007FF"] = "LurkerRight";
        colorToPrefab["0000FFFF"] = "Escalator";
        colorToPrefab["0100FFFF"] = "Elevator";
        colorToPrefab["00A0E9FF"] = "TurnPoint";
        colorToPrefab["00FFFFFF"] = "Direction";
        colorToPrefab["00FF00FF"] = "SavePoint";
        colorToPrefab["FFFF00FF"] = "Heart";
        colorToPrefab["C8C800FF"] = "BinaryDoor";
        colorToPrefab["FF962DFF"] = "CloseDoor";
        colorToPrefab["638C0BFF"] = "MoveTips";
        colorToPrefab["648C0BFF"] = "JumpTips";
        colorToPrefab["658C0BFF"] = "FireTips";
        colorToPrefab["02FFC8FF"] = "SwitchToLevelMap2";


        foreach (string d in Directory.GetFileSystemEntries(pngDirectoryPath, "*.png")) {
            if (File.Exists(d)) {
                GameObject level = new GameObject();
                level.AddComponent<LevelController>();
                Texture2D png = (Texture2D)AssetDatabase.LoadAssetAtPath(d, typeof(Texture2D));
                BuildMapPrefab(level, png, colorToPrefab);
                string levelPath = levelDirectoryPath + "/" + Path.GetFileNameWithoutExtension(d) + ".prefab";
                PrefabUtility.CreatePrefab(levelPath, level);
                GameObject.DestroyImmediate(level);

                AssetDatabase.ImportAsset(levelPath);
                AssetImporter importer = AssetImporter.GetAtPath(levelPath);
                importer.assetBundleName = "level.assetbundle";
                importer.SaveAndReimport();
            }
        }
        AssetDatabase.SaveAssets();

        // Build Bundles
        string assetBundleDirectoryPath = Application.streamingAssetsPath;
        DirectoryInfo assetBundleDirectory = new DirectoryInfo(assetBundleDirectoryPath);
        if (!assetBundleDirectory.Exists) {
            assetBundleDirectory.Create();
        }
        FileInfo[] files = assetBundleDirectory.GetFiles();
        foreach (var item in files) {
            item.Delete();
        }
        #if UNITY_STANDALONE_OSX
        BuildPipeline.BuildAssetBundles(assetBundleDirectoryPath, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneOSXIntel);
        #elif UNITY_STANDALONE_WIN
        BuildPipeline.BuildAssetBundles(assetBundleDirectoryPath, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneWindows64);
        #endif
        Debug.Log("[AssetBundlesBuilder] BuildAssetBundles Done.");
    }

    private static void BuildMapPrefab(GameObject level, Texture2D png, Dictionary<string, string> colorToPrefab)
    {
        Color32[] allPixels = png.GetPixels32();
        int width = png.width;
        int height = png.height;
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                SpawnObjectAt(allPixels, x, y, width, height, colorToPrefab, level);
            }
        }
        level.GetComponent<LevelController>().width = width;
        level.GetComponent<LevelController>().height = height;
    }


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

    private static void SpawnObjectAt(Color32[] pixels, int x, int y, int width, int height, Dictionary<string, string> colorToPrefab, GameObject level)
    {
        Color32 c = pixels[(y * width) + x];
        if (c.a <= 0) return;
        string colorKey = ColorUtility.ToHtmlStringRGBA(c);
        if (colorToPrefab.ContainsKey(colorKey)) {
            string prefab = colorToPrefab[colorKey];
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
                Object obj = AssetDatabase.LoadAssetAtPath("Assets/Bundles/Prefabs/Tiles/" + prefab + ".prefab", typeof(GameObject));
                GameObject go = GameObject.Instantiate(obj, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(level.transform);
            }
            else if (prefab == "PlayerSpawnPoint") {
                level.GetComponent<LevelController>().startPoint = new Vector3(x, y, -10);
            }
            else if (prefab == "Escalator" || prefab == "Elevator") {
                // 加一个中继点
                Object tpObj = AssetDatabase.LoadAssetAtPath("Assets/Bundles/Prefabs/Tiles/TurnPoint.prefab", typeof(GameObject));
                GameObject turnPoint = GameObject.Instantiate(tpObj, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                turnPoint.transform.SetParent(level.transform);

                // 根据周围地块来分析方向
                bool top = y < height - 1 && IsDirectionTile(pixels[((y + 1) * width) + x], colorToPrefab);
                bool bottom = y > 0 && IsDirectionTile(pixels[((y - 1) * width) + x], colorToPrefab);
                bool left = x > 0 && IsDirectionTile(pixels[(y * width) + x - 1], colorToPrefab);
                bool right = x < width - 1 && IsDirectionTile(pixels[(y * width) + x + 1], colorToPrefab);
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
                Object eObj = AssetDatabase.LoadAssetAtPath("Assets/Bundles/Prefabs/Tiles/MovingPlatform.prefab", typeof(GameObject));
                GameObject eGo = GameObject.Instantiate(eObj, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                eGo.transform.SetParent(level.transform);
                if(prefab == "Escalator") {
                    eGo.GetComponent<MovingPlatformController>().auto = true;
                } else if(prefab == "Evelator") {
                    eGo.GetComponent<MovingPlatformController>().auto = false;
                }
            }
            else if (prefab == "TurnPoint") {
                // 加一个中继点
                Object tpObj = AssetDatabase.LoadAssetAtPath("Assets/Bundles/Prefabs/Tiles/TurnPoint.prefab", typeof(GameObject));
                GameObject turnPoint = GameObject.Instantiate(tpObj, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                turnPoint.transform.SetParent(level.transform);

                // 根据周围地块来分析方向
                bool top = IsDirectionTile(pixels[((y + 1) * width) + x], colorToPrefab);
                bool bottom = IsDirectionTile(pixels[((y - 1) * width) + x], colorToPrefab);
                bool left = IsDirectionTile(pixels[(y * width) + x - 1], colorToPrefab);
                bool right = IsDirectionTile(pixels[(y * width) + x + 1], colorToPrefab);

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
            }
            else if (prefab == "Direction") {
                return;
            }
            else if (prefab == "MoveTips" || prefab == "JumpTips" || prefab == "FireTips") {
                Object obj = AssetDatabase.LoadAssetAtPath("Assets/Bundles/Prefabs/Objects/Tips.prefab", typeof(GameObject));
                GameObject go = GameObject.Instantiate(obj, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(level.transform);
                switch(prefab) {
                case "MoveTips":
                go.GetComponent<TipsController>().tipsType = TipsType.Move;
                break;
                case "JumpTips":
                go.GetComponent<TipsController>().tipsType = TipsType.Jump;
                break;
                case "FireTips":
                go.GetComponent<TipsController>().tipsType = TipsType.Fire;
                break;
                default:
                break;
                }
            }
            else if (prefab == "SwitchToLevelMap2") {
                Object obj = AssetDatabase.LoadAssetAtPath("Assets/Bundles/Prefabs/Objects/Switch.prefab", typeof(GameObject));
                GameObject go = GameObject.Instantiate(obj, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(level.transform);
                switch(prefab) {
                case "SwitchToLevelMap2":
                go.GetComponent<SwitchController>().levelName = "LevelMap2";
                break;
                default:
                break;
                }
            }
            else {
                Object obj = AssetDatabase.LoadAssetAtPath("Assets/Bundles/Prefabs/Objects/" + prefab + ".prefab", typeof(GameObject));
                GameObject go = GameObject.Instantiate(obj, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(level.transform);
            }
        }
        else {
            Debug.LogError("[LevelLoader] SpawnTileAt : no color to prefab found for: " + colorKey + ", " + c.ToString() + ", " + x + ", " + y);
        }
    }
}