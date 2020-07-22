using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private CreatureStats _character;
    private Tail _target;
    private bool _isExecuted = true;

    public MoveAction(CreatureStats Character, Tail Target)
    {
        _character = Character;
        _target = Target;
        _character.playerScript.OnMoveFinish += PlayerScript_OnMoveFinish;
    }

    private void PlayerScript_OnMoveFinish()
    {
        _isExecuted = false;
        _character.playerScript.OnMoveFinish -= PlayerScript_OnMoveFinish;
    }

    public override IEnumerator Execute()
    {
        List<Vector3> path = PathFinder.instance.GetPath(_character.transform.position, _target.transform.position, _character.SPD);

        for (int i = 1; i < path.Count; i++)
        {
            GameObject obj = new GameObject("target");
            obj.transform.position = path[i];
            _character.playerScript.SetTarget(obj);
        }

        while (_isExecuted)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);
        status = MatchSystem.actionStatuses.end;
    }

    public override bool Check()
    {
        List<Vector3> path = PathFinder.instance.GetPath(_character.transform.position, _target.transform.position, _character.SPD);
        List<Vector3> fullPath = PathFinder.instance.GetPath(_character.transform.position, _target.transform.position);

        if (path.Count != fullPath.Count)
        {
            return false;
        } else
        {
            return true;
        }
    }
}
