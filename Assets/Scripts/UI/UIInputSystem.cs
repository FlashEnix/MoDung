using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInputSystem : MonoBehaviour
{
    private void Start()
    {
        UISpell.OnClick += UISpell_OnClick;
    }

    private void UISpell_OnClick(UISpell obj)
    {
        SpellSystem.instance.SelectSpell(obj.Creature,obj.Spell);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
