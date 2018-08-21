using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using GlobalDefines;

public class MapPrefabBuilder
{
    [MenuItem("Assets/Generate Map to Prefabs")]
    static void GenerateMapToPrefabs()
    {
        // 根据png来生成level
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
        colorToPrefab["00FF00FF"] = "StartPoint";

        foreach (string d in Directory.GetFileSystemEntries(pngDirectoryPath, "*.png")) {
            if (File.Exists(d)) {
                GameObject level = new GameObject();
                level.AddComponent<LevelContext>().levelName = Path.GetFileNameWithoutExtension(d);
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

        Debug.Log("[MapPrefabBuilder] GenerateMapToPrefabs Done.");
    }

    private static void BuildMapPrefab(GameObject level, Texture2D png, Dictionary<string, string> colorToPrefab)
    {
        Color32[] allPixels = png.GetPixels32();
        int width = png.width;
        int height = png.height;
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                SpawnObjectAt(allPixels, x, y, width, height, colorToPrefab, level);
            }
        }
        level.GetComponent<LevelContext>().width = width;
        level.GetComponent<LevelContext>().height = height;
    }


    private static bool IsColorNotTile(Color32 c, Dictionary<string, string> colorToPrefab)
    {
        string colorKey = ColorUtility.ToHtmlStringRGBA(c);
        if (colorToPrefab.ContainsKey(colorKey) && colorToPrefab[colorKey] == "Tile")
        {
            return false;
        }
        return true;
    }

    private static bool IsDirectionTile(Color32 c, Dictionary<string, string> colorToPrefab)
    {
        string colorKey = ColorUtility.ToHtmlStringRGBA(c);
        if (colorToPrefab.ContainsKey(colorKey) && colorToPrefab[colorKey] == "Direction")
        {
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
            if (prefab == "Tile")
            {
                bool top = y >= height - 1 || IsColorNotTile(pixels[((y + 1) * width) + x], colorToPrefab);
                bool bottom = y <= 0 || IsColorNotTile(pixels[((y - 1) * width) + x], colorToPrefab);
                bool left = x <= 0 || IsColorNotTile(pixels[(y * width) + x - 1], colorToPrefab);
                bool right = x >= width - 1 || IsColorNotTile(pixels[(y * width) + x + 1], colorToPrefab);

                if (top && bottom && left && right)
                {
                    prefab += "Block";
                }
                else if (top && bottom && left && !right)
                {
                    prefab += "PlatformL";
                }
                else if (top && bottom && !left && right)
                {
                    prefab += "PlatformR";
                }
                else if (top && bottom && !left && !right)
                {
                    prefab += "PlatformM";
                }
                else if (top && !bottom && left && right)
                {
                    prefab += "PillarT";
                }
                else if (top && !bottom && left && !right)
                {
                    prefab += "GroundLT";
                }
                else if (top && !bottom && !left && right)
                {
                    prefab += "GroundRT";
                }
                else if (top && !bottom && !left && !right)
                {
                    prefab += "GroundT";
                }
                else if (!top && bottom && left && right)
                {
                    prefab += "PillarB";
                }
                else if (!top && bottom && left && !right)
                {
                    prefab += "GroundLB";
                }
                else if (!top && bottom && !left && right)
                {
                    prefab += "GroundRB";
                }
                else if (!top && bottom && !left && !right)
                {
                    prefab += "GroundB";
                }
                else if (!top && !bottom && left && right)
                {
                    prefab += "PillarM";
                }
                else if (!top && !bottom && left && !right)
                {
                    prefab += "GroundL";
                }
                else if (!top && !bottom && !left && right)
                {
                    prefab += "GroundR";
                }
                else if (!top && !bottom && !left && !right)
                {
                    prefab += "GroundM";
                }
                Object obj = AssetDatabase.LoadAssetAtPath("Assets/Bundles/Prefabs/Tiles/" + prefab + ".prefab", typeof(GameObject));
                GameObject go = GameObject.Instantiate(obj, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(level.transform);
            }
            else if (prefab == "StartPoint")
            {
                level.GetComponent<LevelContext>().startPoint = new Vector3(x, y, -10);
                return;
            }
            else if (prefab == "MoveTips" || prefab == "JumpTips" || prefab == "ShootTips")
            {
                Object obj = AssetDatabase.LoadAssetAtPath("Assets/Bundles/Prefabs/Objects/Tips.prefab", typeof(GameObject));
                GameObject go = GameObject.Instantiate(obj, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(level.transform);
                switch (prefab)
                {
                    case "MoveTips":
                        go.GetComponent<TipsController>().tipsType = TipsType.Move;
                        break;
                    case "JumpTips":
                        go.GetComponent<TipsController>().tipsType = TipsType.Jump;
                        break;
                    case "ShootTips":
                        go.GetComponent<TipsController>().tipsType = TipsType.Shoot;
                        break;
                    default:
                        break;
                }
            }
            else{
                Object obj = AssetDatabase.LoadAssetAtPath("Assets/Bundles/Prefabs/Objects/" + prefab + ".prefab", typeof(GameObject));
                GameObject go = GameObject.Instantiate(obj, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(level.transform);
            }
        }
        else {
            Debug.LogError("[MapPrefabBuilder] SpawnTileAt : no color to prefab found for: " + colorKey + ", " + c.ToString() + ", " + x + ", " + y);
        }
    }
}