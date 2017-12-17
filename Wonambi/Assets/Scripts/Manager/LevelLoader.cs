using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

[System.Serializable]
public class ColorToPrefab {
    public Color32 color;
    public string name;
}
public class LevelLoader : MonoBehaviour
{
    private Texture2D currentLevelMap;

    public ColorToPrefab[] colorToPrefab;

    public Vector2 startPoint;

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

    private bool IsDirectionTile(Color32 c) {
        foreach(var ctp in colorToPrefab) {
            if(ctp.name == "Direction" && ctp.color.Equals(c)) {
                return true;
            }
        }
        return false;
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
                    //Debug.Log("[LevelLoader] SpawnTileAt y = " + y + ", width = " + width);
                    top = y >= height-1 || IsColorNotTile(pixels[((y + 1) * width) + x]);
                    bottom = y <= 0 || IsColorNotTile(pixels[((y - 1) * width) + x]);
                    left = x <= 0 || IsColorNotTile(pixels[(y * width) + x - 1]);
                    right = x >= width-1 || IsColorNotTile(pixels[(y * width) + x + 1]);

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
                    GameObject go = Instantiate(BundleMgr.Instance.GetTile(prefabName), new Vector3(x, y, 0), Quaternion.identity);
                    go.transform.SetParent(transform);
                    return;
                }
                else if(prefabName == "PlayerSpawnPoint") {
                    startPoint = new Vector2(x, y);
                    return;
                }
                else if(prefabName == "Escalator") {
                    // 加一个中继点
                    GameObject tGo = Instantiate(BundleMgr.Instance.GetTile("TurnPoint"), new Vector3(x, y, 0), Quaternion.identity);
                    tGo.transform.SetParent(transform);


                    // 根据周围地块来分析方向
                    top = y < height - 1 && IsDirectionTile(pixels[((y + 1) * width) + x]);
                    bottom = y > 0 && IsDirectionTile(pixels[((y - 1) * width) + x]);
                    left = x > 0 && IsDirectionTile(pixels[(y * width) + x - 1]);
                    right = x < width - 1 && IsDirectionTile(pixels[(y * width) + x + 1]);
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
                    // 加一个电梯
                    GameObject eGo = Instantiate(BundleMgr.Instance.GetTile("MovingPlatform"), new Vector3(x, y, 0), Quaternion.identity);
                    eGo.transform.SetParent(transform);
                    eGo.GetComponent<MovingPlatformController>().auto = true;
                    return;
                }
                else if(prefabName == "TurnPoint") {
                    // 加一个中继点
                    GameObject tGo = Instantiate(BundleMgr.Instance.GetTile("TurnPoint"), new Vector3(x, y, 0), Quaternion.identity);
                    tGo.transform.SetParent(transform);

                    // 根据周围地块来分析方向
                    top = IsDirectionTile(pixels[((y + 1) * width) + x]);
                    bottom = IsDirectionTile(pixels[((y - 1) * width) + x]);
                    left = IsDirectionTile(pixels[(y * width) + x - 1]);
                    right = IsDirectionTile(pixels[(y * width) + x + 1]);
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
                    return;
                }
                else if(prefabName == "Direction") {
                    return;
                }
                else {
                    GameObject go = Instantiate(BundleMgr.Instance.GetObject(prefabName), new Vector3(x, y, 0), Quaternion.identity);
                    go.transform.SetParent(transform);
                    return;
                }
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
