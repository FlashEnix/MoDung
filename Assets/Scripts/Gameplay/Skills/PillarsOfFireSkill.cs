using System.Collections;
using UnityEngine;
public class PillarsOfFireSkill : AoeAction, ISkill
{
    public int Damage = 2;
    public GameObject Trail;
    public string SkillName => "Pillars of Fire";
    public int MP => 2;
    public Sprite Icon => Resources.Load<Sprite>("Samurai_Anger");
    public override bool Check()
    {
        if (Source.MP >= 2) return true;
        else return false;
    }

    public override IEnumerator Execute()
    {
        Source.GetComponent<PlayerScript>().SetAnim("cast");
        if (Trail != null) Trail.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        GameHelper.instance.InstantiateObject("PillarOfFireVfx", Target.transform.position);

        yield return new WaitForSeconds(1);

        Collider[] colliders = Physics.OverlapSphere(Target.transform.position, 1);

        foreach (Collider col in colliders)
        {
            if (!col.isTrigger) continue;
            CreatureStats creature = col.GetComponent<CreatureStats>();
            if (creature != null)
            {
                DamageSystem.instance.DealDamage(Source, creature, Damage, new MagicDamage());
            }
        }

        yield return new WaitForSeconds(1);
        if (Trail != null) Trail.SetActive(false);
        status = MatchSystem.actionStatuses.end;
    }
}