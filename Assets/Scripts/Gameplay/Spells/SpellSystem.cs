using System;
using UnityEngine;

public class SpellSystem : MonoBehaviour
{
    public event Action<ISkill> OnSelectedSpell;
    public event Action<ISkill> OnDeselectedSpell;
    public static SpellSystem instance;
    public ISkill SelectedSpell;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        MatchSystem.instance.OnChangePlayer += MatchSystem_OnChangePlayer;
        InputSystemManager.instance.OnChangeSystem += InputSystemManager_OnChangeSystem;
    }

    private void InputSystemManager_OnChangeSystem()
    {
        ISkill _spell = SelectedSpell;
        SelectedSpell = null;
        OnDeselectedSpell?.Invoke(_spell);
    }

    //Добавляем ману при начале хода
    private void MatchSystem_OnChangePlayer(CreatureStats obj)
    {
        obj.MP += 1;
    }

    public void SelectSpell(CreatureStats creature,ISkill spell)
    {
        if (creature.MP < spell.MP) return;
        SelectedSpell = spell;
        if (SelectedSpell is TargetAction)
        {
            /*Debug.LogFormat("Таргет систем активирована через {0}", SelectedSpell.SkillName);*/
            InputSystemManager.instance.EnableSystem(typeof(TargetInputSystem));
        }
        else if (SelectedSpell is AoeAction)
        {
            InputSystemManager.instance.EnableSystem(typeof(AoeInputSystem));
        }
        OnSelectedSpell?.Invoke(SelectedSpell);
    }
}
