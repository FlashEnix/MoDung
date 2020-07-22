using System.Collections;
using UnityEngine;

public class PillarOfFireAction : AoeAction
{
    public int Damage = 2;
    private int _range = 3;
    private int _distance = 3;

    public override bool Check()
    {
        if (Source.MP >= 2) return true;
        else return false;
    }

    public override IEnumerator Execute()
    {
        Source.MP -= 2;

        PillarOfFireSpell spell = Source.GetComponent<PillarOfFireSpell>();

        Debug.Log(Source.creatureName);

        Source.GetComponent<PlayerScript>().SetAnim("cast");
        if (spell.Trail != null) spell.Trail.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        GameObject prefab = spell.CreatePrefab();
        prefab.transform.position = Target.transform.position;

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
        if (spell.Trail != null) spell.Trail.SetActive(false);
        status = MatchSystem.actionStatuses.end;
    }
}
