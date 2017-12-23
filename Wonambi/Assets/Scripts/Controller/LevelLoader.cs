using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;
public class LevelLoader : MonoBehaviour
{
    private string curLevel;
    private void Start()
    {
        
    }

    public void ClearLevel()
    {
        while(transform.childCount > 0) {
            Transform c = transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }
    }

    public void LoadLevel(string levelName)
    {
        ClearLevel();
        GameObject level = Instantiate(BundleMgr.Instance.GetLevel(name), Vector3.zero, Quaternion.identity);
        level.transform.SetParent(transform);
    }
}
