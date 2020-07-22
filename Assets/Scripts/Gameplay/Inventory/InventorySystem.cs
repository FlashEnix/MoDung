using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySystem: MonoBehaviour
{
    public static InventorySystem instance;

    public event Action<CreatureStats, IInventoryItem> OnPickInventoryItem;
    public event Action<CreatureStats, IInventoryItem> OnRemoveInventoryItem;

    private Dictionary<CreatureStats, Inventory> _creaturesInventory;
    public IInventoryItem SelectedItem;

    private void Awake()
    {
        instance = this;
        _creaturesInventory = new Dictionary<CreatureStats, Inventory>();
    }

    private void Start()
    {
        CreatureStats[] creatures = FindObjectsOfType<CreatureStats>();

        foreach (CreatureStats c in creatures)
        {
            if (!c.GetComponent<AIScript>().isActiveAndEnabled)
            {
                Inventory i = new Inventory(c);
                _creaturesInventory.Add(c,i);
            }
        }

        MatchSystem.instance.OnActionEnd += Match_OnActionEnd;
    }

    private void Match_OnActionEnd(IGameAction action)
    {
        if (action is TargetAction)
        {
            TargetAction targetAction = (TargetAction)action;
            IInventoryItem item = _creaturesInventory[targetAction.Source].ItemList.FirstOrDefault(x => x.GameAction == action);

            if (item != null)
            {
                RemoveItemFromInventory(targetAction.Source, item);
                OnRemoveInventoryItem?.Invoke(targetAction.Source, item);
            }

        }
    }

    public bool AddItemToInventory(CreatureStats creature, IInventoryItem item)
    {
        if (_creaturesInventory[creature].AddItem(item) == true)
        {
            OnPickInventoryItem?.Invoke(creature, item);
            return true;
        }
        return false;
    }

    public void RemoveItemFromInventory(CreatureStats creature, IInventoryItem item)
    {
        _creaturesInventory[creature].RemoveItem(item);
    }

    public bool CheckInventoryFull(CreatureStats creature)
    {
        return _creaturesInventory[creature].CheckFull();
    }

    public void SelectItem(IInventoryItem item)
    {
        Debug.Log(item.ItemName);
        SelectedItem = item;
        if (SelectedItem.GameAction is TargetAction)
        {
            InputSystemManager.instance.EnableSystem(typeof(TargetInputSystem));
        }
        else if (SelectedItem.GameAction is AoeAction)
        {
            InputSystemManager.instance.EnableSystem(typeof(AoeInputSystem));
        }
    }
}
