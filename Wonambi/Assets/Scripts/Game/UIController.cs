using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public GameObject gamePanel;
    public GameObject menuPanel;

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

    public void ShowGamePanel()
    {
        gamePanel.SetActive(true);
    }

    public void HideGamePanel()
    {
        gamePanel.SetActive(false);
    }

    public void ShowMenuPanel()
    {
        menuPanel.SetActive(true);
    }

    public void HideMenuPanel()
    {
        menuPanel.SetActive(false);
    }

    public void StartGame()
    {
        gamePanel.SetActive(true);
        menuPanel.SetActive(false);
    }
}
