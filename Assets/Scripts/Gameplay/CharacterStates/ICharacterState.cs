using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureStats))]
public abstract class ICharacterState : MonoBehaviour
{
    public abstract int CoolDown { get; set; }

    public abstract void Execute();

    public void Run()
    {
        Execute();
        CoolDown -= 1;
        if (CoolDown <= 0)
        {
            Destroy(this);
        }
    }
}
