using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TargetSystem : MonoBehaviour
{
    public event Action<TargetAction,ITargetSource> OnActiveTarget;

    public static TargetSystem instance;


    private TargetAction _action;
    private CreatureStats _target;
    private ITargetSource _source;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        MatchSystem.instance.OnActionEnd += MatchSystem_OnActionEnd;
        ClickObject.OnClick += ClickObject_OnClick;
    }

    private void ClickObject_OnClick(ClickObject obj)
    {
        if (_action != null)
        {
            if (obj.isCreature)
            {
                _action.Init(obj.GetComponent<CreatureStats>());
                MatchSystem.instance.RunAction(_action);
                _action = null;
                return;
            }
        }

        if (obj.isCreature)
        {
            CreatureStats creature = obj.GetComponent<CreatureStats>();

            if (_action == null)
            {
                //MatchSystem.instance.RunAction(new AttackAction(null,creature));
            }
        }
        else if (obj.isTreasure)
        {
            Treasure treasure = obj.GetComponent<Treasure>();
            if (_action == null)
            {
                //MatchSystem.instance.RunAction(new PickTreasureAction(null, treasure));
            }
        }
    }

    private void MatchSystem_OnActionEnd(IGameAction action)
    {
        if ((object)action == _action)
        {
            _action = null;
        }
    }

    void Update()
    {
        if (_action != null)
        {
            //отмена
            if (Input.GetMouseButtonDown(1))
            {
                _action = null;
                OnActiveTarget?.Invoke(_action, _source);
            }
        }
        
    }

    public void ActiveTarget(TargetAction action, ITargetSource source = null)
    {
        _source = source;
        _action = action;
        OnActiveTarget?.Invoke(_action, _source);
    }

    public bool isActive()
    {
        if (_action != null) return true;
        else return false;
    }
}
