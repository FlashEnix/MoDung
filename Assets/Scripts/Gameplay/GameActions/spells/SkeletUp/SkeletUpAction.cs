using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkeletUpAction : BaseAction
{
    public override bool Check()
    {
        if (Source.MP < 2) return false;

        Collider[] tails = Physics.OverlapSphere(Source.transform.position, 2, 1 << 8);

        Tail target = null;

        foreach (Collider col in tails)
        {
            Tail t = col.GetComponent<Tail>();
            if (t.TailType == Tail.TailTypes.Ground)
            {
                target = t;
            }
        }

        if (target == null) return false;
        else return true;
    }

    public override IEnumerator Execute()
    {
        Source.MP -= 2;

        //Определяем тайл на котором спавнить скелета
        Collider[] tails = Physics.OverlapSphere(Source.transform.position, 3, 1 << 8);

        Tail target = null;

        foreach (Collider col in tails)
        {
            Tail t = col.GetComponent<Tail>();
            if (t.TailType == Tail.TailTypes.Ground)
            {
                target = t;
            }
        }

        _animator.SetTrigger("SkeletSkill");

        yield return new WaitForSeconds(1);
        GameObject vfx = GameHelper.instance.InstantiateObject("SkeletUpVfx", target.transform.position);
        yield return new WaitForSeconds(1);
        GameObject skelet = GameHelper.instance.InstantiateObject("Skelet",target.transform.position);
        MatchSystem.instance.AddPlayer(skelet.GetComponent<CreatureStats>());
        yield return new WaitForSeconds(1);
        Destroy(vfx);
        status = MatchSystem.actionStatuses.end;
    }
}
