using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSkill : TargetAction, ISkill
{
    public abstract string SkillName { get; }
    public abstract int MP { get; }
    public abstract Sprite Icon { get; }
}
