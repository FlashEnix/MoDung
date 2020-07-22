using System.Collections;
using UnityEngine;

public interface ISkill
{
    string SkillName { get; }
    int MP { get; }
    Sprite Icon { get; }
}
