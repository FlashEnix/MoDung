using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetAction:BaseAction
{
    public CreatureStats Target { get; protected set; }

    public void Init(CreatureStats target)
    {
        Target = target;
    }
}
