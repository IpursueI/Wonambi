using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;
using UnityEngine.UI;

public class TipsController : MonoBehaviour {

    public TipsType tipsType;
    private Text text;

    private void Awake()
    {
        text = transform.Find("Canvas/Text").gameObject.GetComponent<Text>();
        text.gameObject.SetActive(false);
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
        switch(tipsType) {
        case TipsType.Move:
        text.text = "Press A or D to move your cursor guy.";
        break;
        case TipsType.Jump:
        text.text = "Press K to jump.";
        break;
        case TipsType.Fire:
        text.text = "Press J to fire.";
        break;
        default:
        break;
        }
        text.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        text.gameObject.SetActive(false);
    }
}
