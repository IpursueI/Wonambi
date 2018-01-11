using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;
using UnityEngine.UI;

public class TipsController : MonoBehaviour {

    public TipsType tipsType;

    private void Awake()
    {
    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        string tipsContent;
        switch(tipsType) {
        case TipsType.Move:
        tipsContent = "Press <color=#4299FFFF>A</color> or <color=#4299FFFF>D</color> to move";
        break;
        case TipsType.Jump:
        tipsContent = "Press <color=#4299FFFF>K</color> to jump";
        break;
        case TipsType.Shoot:
        tipsContent = "Press <color=#4299FFFF>J</color> to fire";
        break;
        default:
        tipsContent = "Error!";
        break;
        }
        LevelMgr.Instance.ShowTips(tipsContent, -1.0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        LevelMgr.Instance.HideTips();
    }
}
