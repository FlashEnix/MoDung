using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : ICharacterState
{
    public AudioClip Sound;
    public GameObject Vfx;
    public override int CoolDown { get; set; }

    public override void Execute()
    {
        GameHelper.instance.PlaySoundShot(Sound, .5f);
        if (MatchSystem.instance.GetActivePlayer() == GetComponent<CreatureStats>())
        {
            MatchSystem.instance.RunAction(null);
        }

        if (MatchSystem.instance.GetActivePlayer() == GetComponent<CreatureStats>())
        {
            MatchSystem.instance.RunAction(null);
        }
    }

    private void Start()
    {
        Sound = Resources.Load<AudioClip>("Sound/StunState");
        Vfx = GameHelper.instance.InstantiateObject("StunVfx", transform.position);
        GameHelper.instance.PlaySoundShot(Sound, .5f);
        CoolDown = 1;
    }
}
