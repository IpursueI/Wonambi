using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuPanelController : MonoBehaviour {

    public GameObject startText;
    private GameDirector gameDirector;
	// Use this for initialization
	void Start () {
        startText.GetComponent<Text>().color = Color.white;
        startText.GetComponent<Text>().DOFade(0.1f, 1f).SetLoops(-1, LoopType.Yoyo);
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
    }
	
	// Update is called once per frame
	void Update () {
        if(!gameDirector.IsInGame() && Input.GetKeyDown(KeyCode.Space)) {
            gameDirector.StartCutscene();
        }
	}
}
