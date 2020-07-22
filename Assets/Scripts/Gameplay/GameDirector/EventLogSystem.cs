using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLogSystem : MonoBehaviour
{
    public EventLogSystem instance;
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Start()
    {
        InventorySystem.instance.OnPickInventoryItem += InventorySystem_OnPickInventoryItem;
    }

    private void InventorySystem_OnPickInventoryItem(CreatureStats creature, IInventoryItem item)
    {
        Debug.LogFormat("{0} взял предмет {1}", creature.creatureName, item.ItemName);
    }
}
