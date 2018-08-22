using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;
using SimpleJSON;

public class LevelContext : MonoBehaviour {

    public string levelName;
    public string nextLevel;
    public string bgm;
    public float cameraSize;
    public Vector3 startPoint;
    public float width;
    public float height;
    public bool isCheckPoint;

    private GameObject player;
    private CameraController cameraController;
    private UIController uiController;

    void Awake() {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(JSONClass levelData) {
        uiController = GameObject.Find("UICanvas").GetComponent<UIController>();
        cameraController = GameObject.Find("MainCamera").GetComponent<CameraController>();

        nextLevel = levelData["NextLevel"];
        bgm = levelData["BGM"];
        cameraSize = levelData["CameraSize"].AsFloat;

        //TODO 设置camera属性
        cameraController.SetOrthoSize(cameraSize);
        cameraController.SetScreenSize(width, height);

        // 播放音乐
        GameMgr.Instance.PlayBGM(bgm);

        // 设置下是否现实的部分 TODO
    }

    public void SpawnPlayer()
    {
        if(player == null) {
            player = Instantiate(BundleMgr.Instance.GetObject("Player"), Vector3.zero, Quaternion.identity);
            cameraController.SetPlayer(player);
            GameMgr.Instance.EnableInput();
        }

        //TODO 这里把玩家的保存属性放到player类中
        bool enableDoubleJump = PlayerPrefs.GetInt(PrefsKey.PlayerEnableDoubleJump, 0) > 0;
        int bulletNumber = PlayerPrefs.GetInt(PrefsKey.PlayerBulletNumber, DefineNumber.DefaultBulletNumber);
        int maxHp = PlayerPrefs.GetInt(PrefsKey.PlayerMaxHP, DefineNumber.DefaultHP);
        player.GetComponent<PlayerController>().Init(enableDoubleJump, bulletNumber, maxHp);
        player.transform.position = startPoint;
        player.transform.SetParent(null);
    }

    public void TeleportPlayer()
    {
        if (player == null) {
            Debug.LogError("[LevelMgr] TeleportPlayer failed. player is null.");
            return;
        }

        player.transform.position = startPoint;
    }

     public bool IsPlayerRight(Vector3 monsterPos)
    {
        if (player == null) return false;
        Vector3 pPos = player.transform.position;
        if (pPos.x < monsterPos.x) return false;
        return true;
    }

    public float DistanceToPlayer(Vector3 monsterPos)
    {
        if (player == null) return -1.0f;
        Vector3 pPos = player.transform.position;
        return Mathf.Pow(monsterPos.x - pPos.x, 2) + Mathf.Pow(monsterPos.y - pPos.y, 2);
    }

    public bool IsPlayerClose(Vector3 monsterPos)
    {
        if (player == null) return false;
        Vector3 pPos = player.transform.position;
        float distanceX = Mathf.Abs(monsterPos.x - pPos.x);
        float distanceY = Mathf.Abs(monsterPos.y - pPos.y);
        if(distanceX < DefineNumber.MonsterMoveTriggerDistanceX && distanceY < DefineNumber.MonsterMoveTriggerDistanceY)
        {
            return true;
        }
        return false;
    }

    public bool IsPlayerSoClose(Vector3 monsterPos)
    {
        if (player == null) return false;
        Vector3 pPos = player.transform.position;
        float distanceX = Mathf.Abs(monsterPos.x - pPos.x);
        float distanceY = Mathf.Abs(monsterPos.y - pPos.y);
        if (distanceX < DefineNumber.MonsterFireTriggerDistanceX && distanceY < DefineNumber.MonsterFireTriggerDistanceY)
        {
            return true;
        }
        return false;
    }

    public void RefreshHP()
    {
        if(player != null) {
            int hp = player.GetComponent<PlayerModel>().hp;
            uiController.ShowHp(hp);
        }
    }

    public Vector3 GetPlayerPos()
    {
        if (player == null) return Vector3.zero;
        return player.transform.position;
    }

    public void ShowTips(string content, float duration)
    {
        if(player == null) {
            return;
        }   
        player.GetComponent<PlayerDialogController>().ShowDialog(content, duration);
    }

    public void HideTips()
    {
        if(player == null) {
            return;
        }
        player.GetComponent<PlayerDialogController>().HideDialog();
    }

}
