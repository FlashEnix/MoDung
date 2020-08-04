using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class AI_Lich : AIScript
{
    private CreatureStats _creature;
    private CreatureStats[] _enemies;
    
    void Start()
    {
        _creature = GetComponent<CreatureStats>();
        MatchSystem.instance.OnChangePlayer += MatchSystem_OnChangePlayer;
    }

    private void MatchSystem_OnChangePlayer(CreatureStats creature)
    {
        /*if (creature == _creature)
        {
            Run();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }

    private void Run()
    {
        if (!MatchSystem.instance.CanAction || MatchSystem.instance.GetActivePlayer() != _creature) return;
        _enemies = FindObjectsOfType<CreatureStats>().Where(x => x.team != _creature.team).OrderBy(x => Vector3.Distance(_creature.transform.position, x.transform.position)).ToArray();

        if (_creature.MP >= 2)
        {
            MatchSystem.instance.RunAction(new SkeletUpAction(_creature,_enemies.First().transform.position));
            return;
        }
        else
        {
            Attack();
        }
    }

    private void Attack()
    {
        List<CreatureStats> attackList = new List<CreatureStats>();

        foreach (CreatureStats c in _enemies)
        {
            if (GameHelper.instance.CheckAttack(_creature,c))
            {
                attackList.Add(c);
            }
        }

        if (attackList.Count > 0)
        {
            CreatureStats attack = attackList.First();
            AttackAction action = GetComponent<AttackAction>();
            action.Init(attack);
            MatchSystem.instance.RunAction(action);
        }
        else
        {
            MatchSystem.instance.RunAction(null);
        }
    }
}
