using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISpell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<UISpell> OnHoverIn;
    public static event Action<UISpell> OnHoverOut;
    public static event Action<UISpell> OnClick;
    public Image SelectOverlay;
    public Image notMP;
    public ISkill Spell;
    public CreatureStats Creature;

    private void OnEnable()
    {
        SpellSystem.instance.OnSelectedSpell += SpellSystem_OnSelectedSpell;
        SpellSystem.instance.OnDeselectedSpell += SpellSystem_OnDeselectedSpell;
    }

    private void SpellSystem_OnDeselectedSpell(ISkill obj)
    {
        if (SelectOverlay != null) SelectOverlay.gameObject.SetActive(false);
    }

    private void SpellSystem_OnSelectedSpell(ISkill obj)
    {
        SelectOverlay.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void CalcMP()
    {
        if (Creature.MP < Spell.MP)
        {
            notMP.gameObject.SetActive(true);
        }
        else
        {
            notMP.gameObject.SetActive(false);
        }
    }
}
