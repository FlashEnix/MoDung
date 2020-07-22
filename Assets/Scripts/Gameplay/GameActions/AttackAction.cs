using System.Collections;
using UnityEngine;

public class AttackAction : BaseAction
{   
    private CreatureStats _creatureFrom;
    private CreatureStats _creatureTo;
    private bool _isExecuted = true;

    public AttackAction(CreatureStats from, CreatureStats to)
    {
        if (from == null)
        {
            _creatureFrom = MatchSystem.instance.GetActivePlayer();
        }
        else
        {
            _creatureFrom = from;
        }
        
        _creatureTo = to;
    }

    private void PlayerScript_OnDealDamage()
    {
        _creatureFrom.playerScript.OnDealDamage -= PlayerScript_OnDealDamage;
        int attack = UnityEngine.Random.Range(_creatureFrom.minATK, _creatureFrom.maxATK);
        DamageSystem.instance.DealDamage(_creatureFrom, _creatureTo, attack);
    }

    private void PlayerScript_OnAttackFinish()
    {
        _creatureFrom.playerScript.OnAttackFinish -= PlayerScript_OnAttackFinish;
        _isExecuted = false;
        //status = MatchSystem.actionStatuses.end;
    }

    public override bool Check()
    {
        if (_creatureFrom.team == _creatureTo.team) return false;
        if (_creatureTo.tag == "DeathPlayer") return false;
        float distance = Vector3.Distance(_creatureFrom.transform.position, _creatureTo.transform.position);
        /*Debug.Log(distance);*/
        if (_creatureFrom.rangeAttack == 0)
        {
            if (distance <= 1.6f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if ((int)distance > _creatureFrom.rangeAttack)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public override IEnumerator Execute()
    {
        _creatureFrom.playerScript.OnAttackFinish += PlayerScript_OnAttackFinish;
        _creatureFrom.playerScript.OnDealDamage += PlayerScript_OnDealDamage;
        _creatureFrom.playerScript.SetTarget(_creatureTo.gameObject);
        status = MatchSystem.actionStatuses.start;

        while (_isExecuted)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);
        status = MatchSystem.actionStatuses.end;
    }
}
