using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerSounds : MonoBehaviour
{
    public AudioClip impact;
    public AudioClip death;
    public AudioClip attack;

    private CreatureStats _creature;

    private void Start()
    {
        _creature = GetComponent<CreatureStats>();
        DamageSystem.instance.OnDealDamage += Instance_OnDealDamage;
        DamageSystem.instance.OnDeathPlayer += Instance_OnDeathPlayer;
    }

    private void Instance_OnDeathPlayer(CreatureStats creature, IDamage arg2)
    {
        if (creature == _creature && death != null)
        {
            GameHelper.instance.PlaySoundShot(death);
        }
    }

    private void Instance_OnDealDamage(CreatureStats creature, IDamage arg2)
    {
        if (creature == _creature && impact != null)
        {
            GameHelper.instance.PlaySoundShot(impact);
        }
    }
}
