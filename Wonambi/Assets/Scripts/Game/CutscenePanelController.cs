using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutscenePanelController : MonoBehaviour {
    private bool isFinished;
    private bool isActived;
    private string words;
    public float duration = 0.1f;
    private float timer;
    public Text typeText;
    private int pos = 0;
    private GameDirector gameDirector;

	// Use this for initialization
	void Start () {
        timer = 0;
        words = typeText.text;
        typeText.text = "";
        isActived = true;
        isFinished = false;
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
	}
	
	// Update is called once per frame
	void Update () {
        Type();
        if(isFinished && Input.GetKeyDown(KeyCode.Space)) {
            gameDirector.StartGame();
        }
        if(!isFinished && Input.GetKeyDown(KeyCode.Space)) {
            OnFinish();
        }
	}

    void Type() 
    {
        if(isActived) {
            timer += Time.deltaTime;
            if(timer >= duration) {
                timer = 0;
                pos++;
                typeText.text = words.Substring(0, pos);
                if(pos >= words.Length) {
                    OnFinish();
                }
            }
        }
    }

    void OnFinish()
    {
        isFinished = true;
        isActived = false;
        timer = 0;
        pos = 0;
        typeText.text = words;
    }
}
