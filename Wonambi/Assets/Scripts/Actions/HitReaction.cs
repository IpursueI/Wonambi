using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

[RequireComponent(typeof(SpriteRenderer))]
public class HitReaction : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Color fromColor;
    private Color toColor;
    private bool isAct;
    private bool forward;
    private float timer;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fromColor = spriteRenderer.color;
        timer = DefineNumber.HitBlinkDuration;
        forward = true;
    }
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (!isAct) return;
        if(forward) {
            spriteRenderer.color = toColor;
        } else {
            spriteRenderer.color = fromColor;
        }
        timer -= Time.deltaTime;
        if(timer < 0.0f) {
            timer = DefineNumber.HitBlinkDuration;
            forward = !forward;
        }
    }

    public void Begin(Color clr) 
    {
        if(isAct) {
            timer = DefineNumber.HitBlinkDuration;
        } else {
            fromColor = spriteRenderer.color;
            toColor = clr;
            timer = DefineNumber.HitBlinkDuration;
            forward = true;
            isAct = true;
        }
    }

    public void End()
    {
        spriteRenderer.color = fromColor;
        isAct = false;
    }
}
