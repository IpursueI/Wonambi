using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GamePanelController : MonoBehaviour {

    public List<GameObject> hpList = new List<GameObject>();
    private Image bonfireMask;

    private void Awake()
    {
        bonfireMask = transform.Find("BonfireMask").gameObject.GetComponent<Image>();
        for (int i = 1; i <= 3; ++i) {
            GameObject heartObj = transform.Find("Heart0" + i.ToString()).gameObject;
            heartObj.SetActive(false);
            hpList.Add(heartObj);
        }
    }

    // Use this for initialization
    void Start () {
        bonfireMask.gameObject.SetActive(false);
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

    public void ShowBonfireMask()
    {
        bonfireMask.gameObject.SetActive(true);
        bonfireMask.DOFade(1.0f, 1.0f).OnComplete(EndTween);
    }

    private void EndTween()
    {
        bonfireMask.color = new Color(bonfireMask.color.r, bonfireMask.color.g, bonfireMask.color.b, 0.0f);
        bonfireMask.gameObject.SetActive(false);
    }
}
