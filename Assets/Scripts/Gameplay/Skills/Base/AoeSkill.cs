using System.Collections;
using UnityEngine;

public abstract class AoeSkill: ISkill
{
    protected Tail Target;

    public abstract string SkillName { get; }
    public abstract int MP { get; }
    public abstract Sprite Icon { get; }

    public void Init(Tail target)
    {
        Target = target;
    }
}