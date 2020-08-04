using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINotifyNewItem : MonoBehaviour
{
    public Image Creature;
    public Image Item;

    public void Init(Sprite creature, Sprite item)
    {
        Creature.sprite = creature;
        Item.sprite = item;
    }
}
