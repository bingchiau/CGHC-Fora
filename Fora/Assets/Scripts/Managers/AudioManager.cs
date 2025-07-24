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
        BGM.clip = bgm;
        BGM.Play();
    }

    public void PlaySFX(AudioClip audio)
    {
        SFX.PlayOneShot(audio);
    }

    public void PlayMoving()
    {
        SFX_movement.pitch = Random.Range(0.8f, 1f);
        SFX.PlayOneShot(moving);
    }

    public void PlayShoot()
    {
        SFX_movement.pitch = Random.Range(0.8f, 1f);
        SFX_shoot.PlayOneShot(shoot);
    }

    public void PlayGetHit()
    {
        SFX_getHit.PlayOneShot(getHit);
    }

    public void PlayDash()
    {
        SFX_dash.PlayOneShot(dash);
    }

    public void PlayFireGone()
    {
        SFX_fireGone.PlayOneShot(fireGone);
    }
}
