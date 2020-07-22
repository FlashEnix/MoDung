using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AoeAction : BaseAction
{
    public Tail Target { get; protected set; }

    public void Init(Tail target)
    {
        Target = target;
    }
}
