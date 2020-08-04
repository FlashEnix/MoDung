using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AnimEventController : MonoBehaviour
{
    public event Action OnHitTime;
    public event Action<Vector3> OnShootTime;
    public event Action OnAnimationEnd;

    public AudioClip footSound;
    public AudioClip attackSound;
    private AudioSource audioSource;
    public List<GameObject> animHips;

    private Animator _animator;
    private CreatureStats _creature;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _creature = GetComponentInParent<CreatureStats>();
        _animator = GetComponent<Animator>();
        DamageSystem.instance.OnDealDamage += Instance_OnDealDamage;
        DamageSystem.instance.OnDeathPlayer += Instance_OnDeathPlayer;
    }

    private void Instance_OnDeathPlayer(CreatureStats creature, IDamage arg2)
    {
        if (creature == _creature) _animator.SetBool("death",true);
    }

    private void Instance_OnDealDamage(CreatureStats creature, IDamage arg2)
    {
        if (creature == _creature) _animator.SetTrigger("Impact");
    }

    // Update is called once per frame
    public void dealDamage()
    {
        OnHitTime?.Invoke();
        if (attackSound != null) audioSource.PlayOneShot(attackSound);
    }

    public void endAttack()
    {
        OnAnimationEnd?.Invoke();
    }

    public void shootAttack()
    {
        Vector3 startPosition = transform.position + new Vector3(0, .5f, .5f);

        foreach (GameObject h in animHips)
        {
            if (h.name == "mixamorig:RightHand")
            {
                startPosition = h.transform.position;
            }
        }

        OnShootTime?.Invoke(startPosition);
    }

    public void footStep()
    {
        if (footSound != null) audioSource.PlayOneShot(footSound);
    }
}
