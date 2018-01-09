using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDialogController : MonoBehaviour {

    private GameObject canvas;
    private Text text;

    private void Awake()
    {
        canvas = transform.Find("Canvas").gameObject;
        text = transform.Find("Canvas/Image/Text").gameObject.GetComponent<Text>();
    }

    // Use this for initialization
    void Start () 
    {	 
	}
	
	// Update is called once per frame
	void Update () 
    {	
	}   

    public void ShowDialog(string content, float duration) 
    {
        text.text = content;
        if(duration > 0.0f) {
            StartCoroutine(HideCoroutine(duration));
        }
        canvas.SetActive(true);
    }

    public void HideDialog()
    {
        text.text = "";
        canvas.SetActive(false);
    }

    IEnumerator HideCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        HideDialog();
    }

    public void Rotate(bool forward)
    {
        if(forward) {
            canvas.GetComponent<RectTransform>().localScale = new Vector3(0.01f, 0.01f, 1.0f);
        } else {
            canvas.GetComponent<RectTransform>().localScale = new Vector3(-0.01f, 0.01f, 1.0f);
        }
    }
}
