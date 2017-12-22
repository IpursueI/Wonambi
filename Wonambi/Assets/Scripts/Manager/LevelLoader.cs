using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;
public class LevelLoader : MonoBehaviour
{
    private Texture2D currentLevelMap;
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

    private bool IsColorNotTile(Color32 c)
    {
        string colorKey = ColorUtility.ToHtmlStringRGBA(c);
        if (LevelMgr.Instance.PrefabOfColor(colorKey) == "Tile") 
        {
            return false;
        }
        return true;
    }

    private bool IsDirectionTile(Color32 c) {
        string colorKey = ColorUtility.ToHtmlStringRGBA(c);
        if (LevelMgr.Instance.PrefabOfColor(colorKey) == "Direction") return true;
        return false;
    }

    public void SpawnTileAt(Color32[] pixels, int x, int y, int width, int height)
    {
        Color32 c = pixels[(y * width) + x];
        if (c.a <= 0) return;
        string colorKey = ColorUtility.ToHtmlStringRGBA(c);
        if (LevelMgr.Instance.IsColorToPrefabKeyExists(colorKey)) {
            string prefab = LevelMgr.Instance.PrefabOfColor(colorKey);
            if (prefab == "Tile") {
                bool top = y >= height - 1 || IsColorNotTile(pixels[((y + 1) * width) + x]);
                bool bottom = y <= 0 || IsColorNotTile(pixels[((y - 1) * width) + x]);
                bool left = x <= 0 || IsColorNotTile(pixels[(y * width) + x - 1]);
                bool right = x >= width - 1 || IsColorNotTile(pixels[(y * width) + x + 1]);

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
                GameObject go = Instantiate(BundleMgr.Instance.GetTile(prefab), new Vector3(x, y, 0), Quaternion.identity);
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
