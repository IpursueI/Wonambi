﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : Singleton<GameMgr>
{
    protected GameMgr() { }
    public string identify = "GameMgr";

    private CameraController cameraController;
    private GameDirector gameDirector;
    private UIController uiController;
    private AudioController audioController;
    private bool isInputEnable = false;

    public void Init()
    {
        cameraController = GameObject.Find("MainCamera").GetComponent<CameraController>();
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        uiController = GameObject.Find("UICanvas").GetComponent<UIController>();
        audioController = GameObject.Find("GameDirector").GetComponent<AudioController>();
    }

    public void PlayBGM(string bgm)
    {
        audioController.PlayBGM(bgm);
    }

    public void PlayJumpSFX()
    {
        audioController.PlayJump();
    }

    public void PlayAttackSFX()
    {
        audioController.PlayAttack();
    }

    public void PlayBonfireSFX()
    {
        audioController.Stop();
        audioController.PlayBonfire();
    }

    public void StartGame()
    {
        gameDirector.StartGame();
    }

    public void UIOnStart()
    {
        uiController.ShowMenuUI();
        uiController.gameObject.SetActive(true);
    }

    public void BackToStart()
    {
        uiController.ShowMenuUI();
        LevelMgr.Instance.QuitGame();
        gameDirector.QuitGame();
    }

    public void ShowCutscene()
    {
        uiController.ShowCutsceneUI();
    }

    public void ShowGameUI()
    {
        uiController.ShowGameUI();
    }

    public bool IsInGame()
    {
        return gameDirector.IsInGame();
    }

    public void PlayerMonsterHitSFX()
    {
        audioController.PlayMonsterHit();
    }

    public void PlayMonsterDieSFX()
    {
        audioController.PlayMonsterDie();
    }

    public void PlayPickSFX()
    {
        audioController.PlayItem();
    }

    public void PlayPlayerHitSFX()
    {
        audioController.PlayPlayerHit();
    }

    public void PlayPlayerDieSFX()
    {
        audioController.PlayPlayerDie();
    }

    public void PlayBoss1DieSFX()
    {
        audioController.PlayBoss1Die();
    }

    public void PlayerInvincibleMonsterHitSFX()
    {
        audioController.PlayInvincibleMonsterHit();
    }

    public void EnableInput()
    {
        isInputEnable = true;
    }

    public void DisableInput()
    {
        isInputEnable = false;
    }

    public bool IsInputEnable()
    {
        return isInputEnable;
    }

    public void SetCameraOrthoSize(float oSize)
    {
        cameraController.SetOrthoSize(oSize);
    }

    public void OnDemoBossDie()
    {
        cameraController.OnDemoBossDie();
        audioController.Stop();
        PlayBoss1DieSFX();
        StartCoroutine(EndDemoCoroutine());
    }

    IEnumerator EndDemoCoroutine()
    {
        yield return new WaitForSeconds(4.0f);
        DisableInput();
        uiController.ShowDemoEndUI();
    }

    public void PlayDieFx(Vector3 pos, Color fxColor)
    {
        StartCoroutine(DieFxCoroutine(pos, fxColor));
    }

    IEnumerator DieFxCoroutine(Vector3 pos, Color fxColor)
    {
        GameObject dieOneFx = Instantiate(BundleMgr.Instance.GetObject("DieFxOne"));
        GameObject dieZeroFx = Instantiate(BundleMgr.Instance.GetObject("DieFxZero"));
        dieOneFx.transform.position = pos;
        dieZeroFx.transform.position = pos;
        var oneMain = dieOneFx.GetComponent<ParticleSystem>().main;
        var zeroMain = dieZeroFx.GetComponent<ParticleSystem>().main;
        oneMain.startColor = fxColor;
        zeroMain.startColor = fxColor;
        dieOneFx.GetComponent<ParticleSystem>().Play();
        dieZeroFx.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(0.5f);

        Destroy(dieOneFx);
        Destroy(dieZeroFx);    
    }

    public void PlayPlayerBulletFx(Vector3 pos)
    {
        StartCoroutine(PlayerBulletFxCoroutine(pos));
    }

    IEnumerator PlayerBulletFxCoroutine(Vector3 pos)
    {
        GameObject bulletFx = Instantiate(BundleMgr.Instance.GetObject("PlayerBulletExplosion"));
        bulletFx.transform.position = pos;
        bulletFx.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        Destroy(bulletFx);
    }
}
