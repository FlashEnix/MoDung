using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelScript : MonoBehaviour
{
    public CreatureStats Creature;
    public GameObject Inventory;
    public Image Spell1;
    public Image Spell2;
    public Text PlayerName;
    public Image Portret;

    public void UpdateParams()
    {
        PlayerName.text = Creature.creatureName;
        Portret.sprite = Creature.avatar;

        ISkill[] spells = Creature.GetComponents<ISkill>();

        if (spells.Length == 2)
        {
            Spell1.sprite = spells[0].Icon;
            Spell1.GetComponent<UISpell>().Spell = spells[0];
            Spell1.GetComponent<UISpell>().Creature = Creature;
            Spell1.gameObject.SetActive(true);
            Spell2.sprite = spells[1].Icon;
            Spell2.GetComponent<UISpell>().Spell = spells[1];
            Spell2.GetComponent<UISpell>().Creature = Creature;
            Spell2.gameObject.SetActive(true);
        } 
        else if (spells.Length == 1)
        {
            Spell1.sprite = spells[0].Icon;
            Spell1.GetComponent<UISpell>().Spell = spells[0];
            Spell1.GetComponent<UISpell>().Creature = Creature;
            Spell1.gameObject.SetActive(true);
        }
    }
}
