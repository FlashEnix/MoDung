using System.Collections;
using UnityEngine;

public class PickTreasureAction : BaseAction
{
    private CreatureStats _creature;
    private Treasure _treasure;

    public PickTreasureAction(CreatureStats creature, Treasure treasure)
    {
        if (creature == null)
        {
            _creature = MatchSystem.instance.GetActivePlayer();
        }
        else
        {
            _creature = creature;
        }
        
        _treasure = treasure;
    }

    public override bool Check()
    {
        if (_treasure.empty) return false;
        int distance = (int)Vector3.Distance(_creature.transform.position, _treasure.transform.position);
        if (distance == 1 && !InventorySystem.instance.CheckInventoryFull(_creature))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override IEnumerator Execute()
    {
        status = MatchSystem.actionStatuses.start;
        _treasure.RunAnimation();
        yield return new WaitForSeconds(0.5f);
        IInventoryItem item = GetRandomItem();

        while (!_treasure.isEnd())
        {
            yield return new WaitForSeconds(0.5f);
        }        

        if (InventorySystem.instance.AddItemToInventory(_creature, item) == true)
        {
            status = MatchSystem.actionStatuses.end;
            //Object.Destroy(_treasure.gameObject);
        }
        else
        {
            status = MatchSystem.actionStatuses.cancel;
        }
    }

    private IInventoryItem GetRandomItem()
    {
        return new HealthBottle();
    }
}
    

