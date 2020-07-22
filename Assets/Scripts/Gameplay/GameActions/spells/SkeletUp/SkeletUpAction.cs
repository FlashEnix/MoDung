using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkeletUpAction : BaseAction
{
    private CreatureStats _creature;
    private Vector3 _targetDirection;

    public SkeletUpAction(CreatureStats source,Vector3 targetDirection)
    {
        _creature = source;
        _targetDirection = targetDirection;
    }

    public override bool Check()
    {
        return _creature.MP >= 2;
    }

    public override IEnumerator Execute()
    {
        _creature.MP -= 2;

        //Определяем тайл на котором спавнить скелета
        Collider[] tails = Physics.OverlapSphere(_creature.transform.position, 3, 1 << 8);
        Tail target = tails.Where(x => PathFinder.instance.checkTailFree(x.transform.position)).OrderBy(x => Vector3.Distance(_creature.transform.position, _targetDirection)).FirstOrDefault().GetComponent<Tail>();

        _creature.GetComponent<PlayerScript>().SetAnim("SkeletSkill");
        yield return new WaitForSeconds(1);
        GameObject vfx = GameHelper.instance.InstantiateObject("SkeletUpVfx", target.transform.position);
        yield return new WaitForSeconds(1);
        GameObject skelet = GameHelper.instance.InstantiateObject("Skelet",target.transform.position);
        MatchSystem.instance.AddPlayer(skelet.GetComponent<CreatureStats>());
        yield return new WaitForSeconds(1);
        MonoBehaviour.Destroy(vfx);
        status = MatchSystem.actionStatuses.end;
    }
}
