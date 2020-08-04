using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveAction : AoeAction
{
    private bool _isExecuted = true;

    private void PlayerScript_OnMoveFinish()
    {
        _isExecuted = false;
        //Source.playerScript.OnMoveFinish -= PlayerScript_OnMoveFinish;
    }

    public override IEnumerator Execute()
    {
        List<Vector3> path = PathFinder.instance.GetPath(Source.transform.position, Target.transform.position, Source.SPD);

        for (int i = 1; i < path.Count; i++)
        {
            //Освобождаем клетку, с которой двигается персонаж
            Tail tail = Physics.OverlapSphere(transform.position, 0.2f, 1 << 8).First().GetComponent<Tail>();
            tail.TailType = Tail.TailTypes.Ground;
            tail.Creature = null;

            GameObject obj = new GameObject("target");
            obj.transform.position = path[i];
            Source.playerScript.SetTarget(obj);

            //Source.playerScript.OnMoveFinish += PlayerScript_OnMoveFinish;
        }

        while (Source.playerScript.targetsMove.Count > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);

        //Занимаем клетку
        Target.Creature = Source;
        Target.TailType = Tail.TailTypes.Character;

        status = MatchSystem.actionStatuses.end;
    }

    public override bool Check()
    {
        List<Vector3> path = PathFinder.instance.GetPath(Source.transform.position, Target.transform.position, Source.SPD);
        List<Vector3> fullPath = PathFinder.instance.GetPath(Source.transform.position, Target.transform.position);

        if (path.Count != fullPath.Count)
        {
            return false;
        } else
        {
            return true;
        }
    }
}
