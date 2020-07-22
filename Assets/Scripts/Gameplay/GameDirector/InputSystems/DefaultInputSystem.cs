using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefaultInputSystem : InputSystem
{
    private Tail _playerTail;
    public override void Off()
    {
        foreach (Tail t in PathFinder.instance.getLastResult())
        {
            t.SetColor(Color.white);
        }
    }

    public override void On()
    {
        _playerTail = GameHelper.instance.GetTailFromObject(MatchSystem.instance.GetActivePlayer().gameObject);
    }

    public override void OnClick(ClickObject obj)
    {
        if (obj.isTail)
        {
            MoveAction action = new MoveAction(MatchSystem.instance.GetActivePlayer(), obj.GetComponent<Tail>());
            MatchSystem.instance.RunAction(action);
        }
        else if (obj.isTreasure)
        {
            MatchSystem.instance.RunAction(new PickTreasureAction(null, obj.GetComponent<Treasure>()));
        }
        else if (obj.isCreature)
        {
            MatchSystem.instance.RunAction(new AttackAction(null, obj.GetComponent<CreatureStats>()));
        }
    }

    public override void OnHoverIn(ClickObject obj)
    {
        if (obj.isTail)
        {
            TailHover(obj.GetComponent<Tail>());
        }
    }

    public override void OnHoverOut(ClickObject obj)
    {
        if (obj.isTail)
        {
            foreach (Tail t in PathFinder.instance.getLastResult())
            {
                t.SetColor(Color.white);
            }
            obj.GetComponent<Tail>().SetColor(Color.white);
        }
    }

    private void TailHover(Tail tail)
    {
        tail.SetColor(Color.blue);

        List<Tail> path = PathFinder.instance.getPath(_playerTail, tail, MatchSystem.instance.GetActivePlayer().SPD);

        foreach (Tail t in path)
        {
            if (t == tail) t.SetColor(Color.green);
            else
            {
                t.SetColor(Color.yellow);
            }
        }
    }
}
