using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Inventory
{
    private int _limit = 2;
    private CreatureStats _creature;
    public List<IInventoryItem> ItemList;

    public Inventory(CreatureStats creature, int limit = 2)
    {
        ItemList = new List<IInventoryItem>();

        _creature = creature;
        _limit = limit;
    }

    public bool AddItem(IInventoryItem item)
    {
        if (CheckFull()) return false;
        else
        {
            ItemList.Add(item);
            return true;
        }
    }

    public void RemoveItem(IInventoryItem item)
    {
        ItemList.Remove(item);
    }

    public bool CheckFull()
    {
        return ItemList.Count == _limit;
    }
}
