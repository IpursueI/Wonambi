using System.Collections;using System.Collections.Generic;using UnityEngine;public class FPSController : MonoBehaviour {

    private float m_LastUpdateShowTime = 0f;    //上一次更新帧率的时间;

    private float m_UpdateShowDeltaTime = 0.01f;//更新帧率的时间间隔;

    private int m_FrameUpdate = 0;//帧数;

    private float m_FPS = 0;    private void Awake()    {        Application.targetFrameRate = 30;    }    // Use this for initialization    void Start()    {        m_LastUpdateShowTime = Time.realtimeSinceStartup;    }    // Update is called once per frame    void Update()    {        m_FrameUpdate++;        if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)        {            m_FPS = m_FrameUpdate / (Time.realtimeSinceStartup - m_LastUpdateShowTime);            m_FrameUpdate = 0;            m_LastUpdateShowTime = Time.realtimeSinceStartup;        }    }    void OnGUI()    {
        GUIStyle bb = new GUIStyle();
        bb.normal.background = null;    //这是设置背景填充的
        bb.normal.textColor = new Color(1.0f, 0.5f, 0.0f);   //设置字体颜色的
        bb.fontSize = 100;       //当然，这是字体大小

        GUI.Label(new Rect(Screen.width / 2, 0, 400, 400), "FPS: " + m_FPS, bb);    }}