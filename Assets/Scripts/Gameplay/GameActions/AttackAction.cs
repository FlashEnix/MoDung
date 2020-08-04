using System.Collections;
using UnityEngine;

public class AttackAction : TargetAction
{   
    private bool _isExecuted = true;

    private void PlayerScript_OnDealDamage()
    {
        Source.playerScript.OnDealDamage -= PlayerScript_OnDealDamage;
        int attack = UnityEngine.Random.Range(Source.minATK, Source.maxATK);
        DamageSystem.instance.DealDamage(Source, Target, attack);
    }

    private void PlayerScript_OnAttackFinish()
    {
        Source.playerScript.OnAttackFinish -= PlayerScript_OnAttackFinish;
        _isExecuted = false;
    }

    public override bool Check()
    {
        if (Source.team == Target.team) return false;
        if (Target.tag == "DeathPlayer") return false;
        float distance = Vector3.Distance(Source.transform.position, Target.transform.position);
        
        if (Source.rangeAttack == 0)
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
            if ((int)distance > Source.rangeAttack)
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
        Source.playerScript.OnAttackFinish += PlayerScript_OnAttackFinish;
        Source.playerScript.OnDealDamage += PlayerScript_OnDealDamage;
        Source.playerScript.SetTarget(Target.gameObject);
        status = MatchSystem.actionStatuses.start;

        while (_isExecuted)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);
        status = MatchSystem.actionStatuses.end;
    }
}
