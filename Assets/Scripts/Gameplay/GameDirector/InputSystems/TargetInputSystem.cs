using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInputSystem : InputSystem
{
    private IInventoryItem _activeItem;
    private TargetAction _activeSpell;
    public override void Off()
    {
    }

    public override void On()
    {
        _activeItem = InventorySystem.instance.SelectedItem;
        _activeSpell = (TargetAction)SpellSystem.instance.SelectedSpell;
    }

    public override void OnClick(Tail obj)
    {
        if (obj.TailType == Tail.TailTypes.Character)
        {
            _activeSpell.Init(obj.Creature);
            MatchSystem.instance.RunAction(_activeSpell);
        }
        
    }

    public override void OnHoverIn(Tail tail)
    {
        /*if (tail.TailType == Tail.TailTypes.Character)
        {
            if (_activeSpell.Check)
        }*/
    }

    public override void OnHoverOut(Tail tail)
    {
        
    }
}
