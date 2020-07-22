using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HealthAction : TargetAction
{
    private int _hp;

    public HealthAction(int HP)
    {
        _hp = HP;
    }

    public override bool Check()
    {
        if (Source.team == Target.team)
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
        Source.GetComponent<PlayerScript>().SetAnim("castSelf");
        yield return new WaitForSeconds(1);
        GameObject effect = EffectLoad();
        yield return new WaitForSeconds(2);
        DamageSystem.instance.Heal(Source, Target, _hp);
        yield return new WaitForSeconds(1);
        MonoBehaviour.Destroy(effect);
        status = MatchSystem.actionStatuses.end;
    }

    private GameObject EffectLoad()
    {
        GameObject effect = VfxDic.instance.CreateEffect("HealEffect");
        if (effect != null)
        {
            effect.transform.position = Target.transform.position;
            return effect;
        } else
        {
            return null;
        }
        
    }
}
