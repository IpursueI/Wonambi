using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorToPrefab {
    public Color32 color;
    public string name;
}
public class LevelLoader : MonoBehaviour
{
    private Texture2D currentLevelMap;

    public ColorToPrefab[] colorToPrefab;

    private void Start()
    {
    }

    private void Clear()
    {
        while(transform.childCount > 0) {
            Transform c = transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }
    }

    private string PrefabNameOfColor(Color32 c)
    {
        if (c.a <= 0) return "";
        foreach(var ctp in colorToPrefab) {
            if(ctp.color.Equals(c)) {
                return ctp.name;
            }
        }
        return "";
    }

    private bool IsColorNotTile(Color32 c)
    {
        foreach(var ctp in colorToPrefab) {
            if(ctp.name == "Tile" && ctp.color.Equals(c)) {
                return false;
            }
        }
        return true;
    }

    public void SpawnTileAt(Color32[] pixels, int x, int y, int width, int height) 
    {
        Color32 c = pixels[(y * width) + x];
        if (c.a <= 0) return;
        bool top = false;
        bool bottom = false;
        bool left = false;
        bool right = false;
        foreach (var ctp in colorToPrefab) {
            if (ctp.color.Equals(c)) {
                string prefabName = ctp.name;
                if (prefabName == "Tile") {
                    top = y >= height || IsColorNotTile(pixels[((y + 1) * width) + x]);
                    bottom = y <= 0 || IsColorNotTile(pixels[((y - 1) * width) + x]);
                    left = x <= 0 || IsColorNotTile(pixels[(y * width) + x - 1]);
                    right = x >= width || IsColorNotTile(pixels[(y * width) + x + 1]);

                    if (top && bottom && left && right) {
                        prefabName += "Block";
                    }
                    else if (top && bottom && left && !right) {
                        prefabName += "PlatformL";
                    }
                    else if (top && bottom && !left && right) {
                        prefabName += "PlatformR";
                    }
                    else if (top && bottom && !left && !right) {
                        prefabName += "PlatformM";
                    }
                    else if (top && !bottom && left && right) {
                        prefabName += "PillarT";
                    }
                    else if (top && !bottom && left && !right) {
                        prefabName += "GroundLT";
                    }
                    else if (top && !bottom && !left && right) {
                        prefabName += "GroundRT";
                    }
                    else if (top && !bottom && !left && !right) {
                        prefabName += "GroundT";
                    }
                    else if (!top && bottom && left && right) {
                        prefabName += "PillarB";
                    }
                    else if (!top && bottom && left && !right) {
                        prefabName += "GroundLB";
                    }
                    else if (!top && bottom && !left && right) {
                        prefabName += "GroundRB";
                    }
                    else if (!top && bottom && !left && !right) {
                        prefabName += "GroundB";
                    }
                    else if (!top && !bottom && left && right) {
                        prefabName += "PillarM";
                    }
                    else if (!top && !bottom && left && !right) {
                        prefabName += "GroundL";
                    }
                    else if (!top && !bottom && !left && right) {
                        prefabName += "GroundR";
                    }
                    else if (!top && !bottom && !left && !right) {
                        return;
                    }
                }
                GameObject go = Instantiate(BundleMgr.Instance.GetTile(prefabName), new Vector3(x, y, 0), Quaternion.identity);
                go.transform.SetParent(transform);
                return;
            }
        }
        Debug.LogError("[LevelLoader] SpawnTileAt : no color to prefab found for: " + c.ToString() + ", " + x + ", " + y);
    }
    public void LoadLevel(string levelName)
    {
        Clear();
        currentLevelMap = BundleMgr.Instance.GetLevelMap(levelName);

        Color32[] allPixels = currentLevelMap.GetPixels32();
        int width = currentLevelMap.width;
        int height = currentLevelMap.height;
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                SpawnTileAt(allPixels, x, y, width, height);
            }
        }
    }
}
