using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanelController : MonoBehaviour {

    public List<GameObject> hpList = new List<GameObject>();

    private void Awake()
    {
        for (int i = 1; i <= 3; ++i) {
            GameObject heartObj = transform.Find("Heart0" + i.ToString()).gameObject;
            heartObj.SetActive(false);
            hpList.Add(heartObj);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowHP(int hp)
    {
        if (hp > 3) return;
        for (int i = 0; i < hpList.Count; ++i) {
            if (i < hp) {
                hpList[i].SetActive(true);
            }
            else {
                hpList[i].SetActive(false);
            }
        }
    }
}
