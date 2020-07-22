using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureStats))]
public abstract class ISpell: MonoBehaviour
{
    public abstract string SpellName { get; }
    public abstract Sprite Image { get; }
    public abstract int MP { get; }

    public abstract IGameAction GameAction {get;}
}
