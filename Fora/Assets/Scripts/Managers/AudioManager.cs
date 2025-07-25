using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("AudioSource")]
    public AudioSource BGM;
    public AudioSource SFX;
    public AudioSource SFX_movement;
    public AudioSource SFX_shoot;
    public AudioSource SFX_getHit;
    public AudioSource SFX_dash;
    public AudioSource SFX_fireGone;

    [Header("AudioClips")]
    public AudioClip bgm;
    public AudioClip shoot;
    public AudioClip hitEnemy;
    public AudioClip hitFire;
    public AudioClip suckWater;
    public AudioClip moving;
    public AudioClip dash;
    public AudioClip getHit;
    public AudioClip enemyDie;
    public AudioClip hogDash;
    public AudioClip nut;
    public AudioClip explode;
    public AudioClip die;
    public AudioClip teleport;
    public AudioClip pickUp;
    public AudioClip fireGone;

    private void Start()
    {
        if (bgm == null) return;
        BGM.clip = bgm;
        BGM.Play();
    }

    public void PlaySFX(AudioClip audio)
    {
        if (audio == null) return;
        SFX.PlayOneShot(audio);
    }

    public void PlayMoving()
    {
        if (moving == null) return;
        SFX_movement.pitch = Random.Range(0.8f, 1f);
        SFX.PlayOneShot(moving);
    }

    public void PlayShoot()
    {
        if (shoot == null) return;
        SFX_shoot.pitch = Random.Range(0.8f, 1f);
        SFX_shoot.PlayOneShot(shoot);
    }

    public void PlayGetHit()
    {
        if (getHit == null) return;
        SFX_getHit.PlayOneShot(getHit);
    }

    public void PlayDash()
    {
        if (dash == null) return;
        SFX_dash.PlayOneShot(dash);
    }

    public void PlayFireGone()
    {
        if (fireGone == null) return;
        SFX_fireGone.PlayOneShot(fireGone);
    }
}
