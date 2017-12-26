using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
    
    public GameObject menuPanel;
    public GameObject cutscenePanel;
    public GameObject gamePanel;

    private void Awake()
    {

    }
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowHp(int hp) {
        if(gamePanel.activeSelf) {
            gamePanel.GetComponent<GamePanelController>().ShowHP(hp);
        }
    }

    public void ShowMenuUI()
    {
        menuPanel.SetActive(true);
        cutscenePanel.SetActive(false);
        gamePanel.SetActive(false);
    }

    public void ShowCutsceneUI()
    {
        menuPanel.SetActive(false);
        cutscenePanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void ShowGameUI()
    {
        menuPanel.SetActive(false);
        cutscenePanel.SetActive(false);
        gamePanel.SetActive(true);
    }
}
