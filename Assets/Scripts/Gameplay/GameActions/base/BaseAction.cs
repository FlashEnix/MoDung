using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureStats))]
public abstract class BaseAction : MonoBehaviour, IGameAction
{
    public MatchSystem.actionStatuses status { get; set; }

    public CreatureStats Source { get; set; }

    public abstract bool Check();

    public virtual void BeforeAction() { }
    public abstract IEnumerator Execute();

    private void Start()
    {
        Source = GetComponent<CreatureStats>();
    }
}
