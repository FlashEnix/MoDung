using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHitAction : TargetAction
{
    public override bool Check()
    {
        if (Source.MP >= 2)
        {
            return GameHelper.instance.CheckAttack(Source, Target);
        } 
        else
        {
            return false;
        }
        
    }

    public override IEnumerator Execute()
    {
        Source.MP -= 2;
        Source.transform.LookAt(new Vector3(Target.transform.position.x, Source.transform.position.y, Target.transform.position.z));
        Source.GetComponent<PlayerScript>().SetAnim("shieldAttack");
        yield return new WaitForSeconds(1);

        StunState check = Target.gameObject.GetComponent<StunState>();
        if (check != null) MonoBehaviour.Destroy(check);

        Target.gameObject.AddComponent<StunState>();
        yield return new WaitForSeconds(1);
        status = MatchSystem.actionStatuses.end;
    }
}
