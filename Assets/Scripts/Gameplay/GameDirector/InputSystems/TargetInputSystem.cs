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

    public override void OnClick(ClickObject obj)
    {
        if (obj.isCreature)
        {
            _activeSpell.Init(obj.GetComponent<CreatureStats>());
            MatchSystem.instance.RunAction(_activeSpell);
        }
        
    }

    public override void OnHoverIn(ClickObject obj)
    {
        
    }

    public override void OnHoverOut(ClickObject obj)
    {
        
    }
}
