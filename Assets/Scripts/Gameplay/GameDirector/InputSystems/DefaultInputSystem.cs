using System;
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

    public override void OnClick(Tail obj)
    {
        if (obj.TailType == Tail.TailTypes.Ground)
        {
            MoveAction action = MatchSystem.instance.GetActivePlayer().GetComponent<MoveAction>();
            action.Init(obj);
            MatchSystem.instance.RunAction(action);
        }
        else if (obj.TailType == Tail.TailTypes.Treasure)
        {
            PickTreasureAction action = MatchSystem.instance.GetActivePlayer().GetComponent<PickTreasureAction>();
            action.Init(obj.Treasure);
            MatchSystem.instance.RunAction(action);

            //MatchSystem.instance.RunAction(new PickTreasureAction(null, obj.Treasure));
        }
        else if (obj.TailType == Tail.TailTypes.Character)
        {
            AttackAction action = MatchSystem.instance.GetActivePlayer().GetComponent<AttackAction>();
            action.Init(obj.Creature);
            MatchSystem.instance.RunAction(action);
        }
    }

    public override void OnHoverIn(Tail obj)
    {
        if (obj.TailType == Tail.TailTypes.Ground)
        {
            TailHover(obj);
        } else if (obj.TailType == Tail.TailTypes.Character)
        {
            if (GameHelper.instance.CheckAttack(MatchSystem.instance.GetActivePlayer(), obj.Creature))
            {
                obj.SetColor(Color.green);
            } else
            {
                obj.SetColor(Color.red);
            }
        } else if (obj.TailType == Tail.TailTypes.Treasure)
        {
            PickTreasureAction checkAction = MatchSystem.instance.GetActivePlayer().GetComponent<PickTreasureAction>();
            checkAction.Init(obj.Treasure);
            if (checkAction.Check()) {
                obj.SetColor(Color.green);
            } else
            {
                obj.SetColor(Color.red);
            }
        }
    }

    public override void OnHoverOut(Tail obj)
    {
        foreach (Tail t in PathFinder.instance.getLastResult())
        {
            t.SetColor(Color.white);
        }
       
        obj.SetColor(Color.white);
    }

    private void TailHover(Tail tail)
    {
        tail.SetColor(Color.red);

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
