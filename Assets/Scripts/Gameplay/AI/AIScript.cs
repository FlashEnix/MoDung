using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CreatureStats))]
public class AIScript : MonoBehaviour
{
    private CreatureStats _creatureStat;

    // Start is called before the first frame update
    void Start()
    {
        _creatureStat = GetComponent<CreatureStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MatchSystem.instance.GetActivePlayer() == _creatureStat && MatchSystem.instance.CanAction)
        {
            
            List<CreatureStats> enemies = MatchSystem.instance.GetAllPlayers()
                .Where(x => x != _creatureStat)
                .Where(x => x.team != _creatureStat.team).ToList();
            //Debug.LogFormat("AI нашел {0} врагов", enemies.Count);

            if (enemies.Count > 0)
            {
                AIPriority enemy = checkPrority(enemies);
                if (enemy == null) return;

                if (enemy.isAttack)
                {
                    //AttackAction action = new AttackAction(_creatureStat, enemy.enemy);
                    AttackAction action = GetComponent<AttackAction>();
                    action.Init(enemy.enemy);
                    MatchSystem.instance.RunAction(action);
                }
                else
                {
                    //MoveAction action = new MoveAction(_creatureStat, enemy.tail);
                    MoveAction action = GetComponent<MoveAction>();
                    action.Init(enemy.tail);
                    MatchSystem.instance.RunAction(action);
                }
            }
        }
    }

    AIPriority checkPrority(List<CreatureStats> enemies)
    {
        List<AIPriority> priorityList = new List<AIPriority>();

        foreach (CreatureStats enemy in enemies)
        {
            Tail tailEnemy = getTail(enemy.transform.position);
            AIPriority aip = new AIPriority();

            //Проверяем на возможность атаковать
            if (GameHelper.instance.CheckAttack(_creatureStat,enemy))
            {
                aip.enemy = enemy;
                aip.distance = 1;
                aip.tail = tailEnemy;
                aip.isAttack = true;
                priorityList.Add(aip);
                continue;
            }

            //Логика приоритета для дальних персов
            if (_creatureStat.rangeAttack > 0)
            {
                //Ищем ближайшую точку, с которой сможем попасть по врагу
                Collider[] cols = Physics.OverlapSphere(enemy.transform.position, ((float)_creatureStat.rangeAttack) - 0.4f, 1 << 8);

                Collider minCol = cols.OrderBy(x => Vector3.Distance(x.transform.position, gameObject.transform.position)).FirstOrDefault();

                List<Vector3> path = PathFinder.instance.GetPath(getTail(_creatureStat.transform.position).transform.position, minCol.transform.position, _creatureStat.SPD);

                if (path.Count > 0)
                {
                    aip.path = path;
                    aip.enemy = enemy;
                    aip.distance = path.Count;
                    aip.tail = getTail(path.Last());
                    priorityList.Add(aip);
                    continue;
                }
            }
            //Логика для ближнего бойца
            List<Vector3> pathToEnemy = PathFinder.instance.GetPath(getTail(_creatureStat.transform.position).transform.position, tailEnemy.transform.position,_creatureStat.SPD);

            if (pathToEnemy.Count == 0) continue;

            List<Vector3> allPath = PathFinder.instance.GetPath(getTail(_creatureStat.transform.position).transform.position, tailEnemy.transform.position);

            if (allPath.Count == pathToEnemy.Count)
            {
                pathToEnemy.RemoveAt(pathToEnemy.Count - 1);
            }

            aip.path = pathToEnemy;
            aip.enemy = enemy;
            aip.distance = pathToEnemy.Count;
            aip.fullDistance = allPath.Count;
            aip.tail = getTail(pathToEnemy.Last());

            priorityList.Add(aip);
        }

        AIPriority first = priorityList.OrderBy(x => x.distance - (x.isAttack?1:0) + x.fullDistance).FirstOrDefault();
        return first;
    }

    Tail getTail(Vector3 v)
    {
        Collider[] colliders = Physics.OverlapSphere(v, 0.1f, 1 << 8);

        if (colliders.Length > 0) return colliders[0].gameObject.GetComponent<Tail>();
        return null;
    }
}

class AIPriority
{
    public CreatureStats enemy;
    public Tail tail;
    public List<Vector3> path;
    public int distance;
    public int fullDistance;
    public bool isAttack;
}
