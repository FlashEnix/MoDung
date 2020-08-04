using System.Collections;
using UnityEngine;

public class ShieldHitSkill : TargetAction, ISkill
{
    public string SkillName => "Shield Hit";

    public int MP => 2;

    public Sprite Icon => Resources.Load<Sprite>("Samurai_Sword Strike");

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
        Source.transform.LookAt(new Vector3(Target.transform.position.x, Source.transform.position.y, Target.transform.position.z));
        _animator.SetTrigger("shieldAttack");
        yield return new WaitForSeconds(1);

        StunState check = Target.GetComponent<StunState>();
        if (check != null) Destroy(check);

        Target.gameObject.AddComponent<StunState>();
        yield return new WaitForSeconds(1);
        status = MatchSystem.actionStatuses.end;
    }
}
