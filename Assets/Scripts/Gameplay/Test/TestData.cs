using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour
{
    public CreatureStats Player;
    public CreatureStats Friend;
    // Start is called before the first frame update
    void Start()
    {
        InventorySystem.instance.AddItemToInventory(Player, new HealthBottle());
        /*DamageSystem.instance.DealDamage(Player, Friend, 2);*/
        Friend.HP -= 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
