using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuPanelController : MonoBehaviour {

    public GameObject startText;
	// Use this for initialization
	void Start () {
        startText.GetComponent<Text>().color = Color.white;
        startText.GetComponent<Text>().DOFade(0.1f, 1.0f).SetLoops(-1, LoopType.Yoyo);
    }
	
	// Update is called once per frame
	void Update () {
        if(!GameMgr.Instance.IsInGame() && Input.GetKeyDown(KeyCode.Space)) {
            GameMgr.Instance.ShowCutscene();
        }
	}

}
