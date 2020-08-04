using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Obsolete("Старая система")]
public class TailSystem : MonoBehaviour
{
    public static TailSystem instance;
    public event Action<Tail,List<Tail>> OnCalculatePath;

    private Tail[] _tails;
    private Tail _playerTail;

    private void Awake()
    {
        instance = this;
        _tails = FindObjectsOfType<Tail>();
    }

    void Start()
    {
        Tail.OnHoverIn += Tail_OnHoverIn;
        Tail.OnHoverOut += Tail_OnHoverOut;
        Tail.OnClick += Tail_OnClick;
        MatchSystem.instance.OnChangePlayer += Match_OnChangePlayer;
        MatchSystem.instance.OnActionEnd += Instance_OnActionEnd;
        _playerTail = GetTailObject(MatchSystem.instance.GetActivePlayer().gameObject);
    }

    private void Tail_OnClick(Tail tail)
    {
        /*if (!SingleSystemManager.instance.CheckOn(GetType())) return;*/
        //MoveAction action = new MoveAction(MatchSystem.instance.GetActivePlayer(), tail);
        MoveAction action = MatchSystem.instance.GetActivePlayer().GetComponent<MoveAction>();
        action.Init(tail);
        MatchSystem.instance.RunAction(action);
    }

    private void Match_OnChangePlayer(CreatureStats creature)
    {
        _playerTail = GetTailObject(creature.gameObject);
    }

    private void Instance_OnActionEnd(IGameAction obj)
    {
        _playerTail = GetTailObject(MatchSystem.instance.GetActivePlayer().gameObject);
    }

    private void Tail_OnHoverOut(Tail tail)
    {
        foreach (Tail t in PathFinder.instance.getLastResult())
        {
            t.SetColor(Color.white);
        }
        tail.SetColor(Color.white);
    }

    private void Tail_OnHoverIn(Tail tail)
    {
        if (!MatchSystem.instance.CanAction) return;
        /*if (!SingleSystemManager.instance.CheckOn(GetType())) return;*/
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

    private Tail GetTailObject(GameObject obj)
    {
        return _tails.OrderBy(x => Vector3.Distance(obj.transform.position, x.transform.position)).FirstOrDefault();
    }

    private Tail GetTailObject(Vector3 position)
    {
        return _tails.OrderBy(x => Vector3.Distance(position, x.transform.position)).FirstOrDefault();
    }

    public List<Vector3> GetPath(Vector3 start, Vector3 end, int limit = 1000)
    {
        List<Vector3> result = new List<Vector3>();
        Tail startTail = GetTailObject(start);
        Tail endTail = GetTailObject(end);

        List<Tail> path = PathFinder.instance.getPath(startTail, endTail, limit);
        
        foreach (Tail t in path)
        {
            result.Add(t.transform.position);
        }
        return result;
    }
}
