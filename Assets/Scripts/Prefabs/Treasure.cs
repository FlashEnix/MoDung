using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour, ITailObject
{
    private Animator _anim;
    private AudioSource _audioSource;
    private bool _end = false;

    public bool empty = false;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void RunAnimation()
    {
        _anim.enabled = true;
        _audioSource.Play();
    }

    public void EndAnimation()
    {
        _end = true;
        empty = true;
    }

    public bool isEnd()
    {
        return _end;
    }
}
