using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefines;

public class AudioController : MonoBehaviour
{
    public AudioClip bgm;
    public AudioClip bossBgm;
    public AudioClip attack;
    public AudioClip playerHit;
    public AudioClip monsterHit;
    public AudioClip item;
    public AudioClip bonfire;
    public AudioClip jump;
    public AudioClip playerDie;
    public AudioClip monsterDie;
    public AudioClip boss1Die;
    public AudioClip invincibleMonsterHit;

    private bool isMute;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0.5f;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayBGM()
    {
        audioSource.Stop();
        audioSource.clip = bgm;
        audioSource.PlayDelayed(0.3f);

    }

    public void PlayBossBGM()
    {
        audioSource.Stop();
        audioSource.clip = bossBgm;
        audioSource.PlayDelayed(0.5f);
    }

    public void PlayAttack()
    {
        audioSource.PlayOneShot(attack);
    }

    public void PlayPlayerHit()
    {
        audioSource.PlayOneShot(playerHit);
    }

    public void PlayMonsterHit()
    {
        audioSource.PlayOneShot(monsterHit);
    }

    public void PlayItem()
    {
        audioSource.PlayOneShot(item);
    }

    public void PlayBonfire()
    {
        audioSource.PlayOneShot(bonfire);
    }

    public void PlayJump()
    {
        audioSource.PlayOneShot(jump);
    }

    public void PlayPlayerDie()
    {
        audioSource.PlayOneShot(playerDie);
    }

    public void PlayMonsterDie()
    {
        audioSource.PlayOneShot(monsterDie);
    }

    public void PlayBoss1Die()
    {
        audioSource.PlayOneShot(boss1Die);
    }

    public void PlayInvincibleMonsterHit()
    {
        audioSource.PlayOneShot(invincibleMonsterHit);
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void UnPause()
    {
        audioSource.UnPause();
    }
}
