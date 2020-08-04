using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveAction : AoeAction
{
    private Transform _transform;
    private float _moveSpeed = 2;

    private new void Start()
    {
        base.Start();
        _transform = transform;
    }

    public override IEnumerator Execute()
    {
        //Освобождаем клетку, с которой двигается персонаж
        Tail startTale = Physics.OverlapSphere(_transform.position, 0.2f, 1 << 8).First().GetComponent<Tail>();
        startTale.TailType = Tail.TailTypes.Ground;
        startTale.Creature = null;

        List<Tail> path = PathFinder.instance.getPath(startTale, Target, Source.SPD);

        _animator.SetBool("walk", true);

        foreach (Tail nextTail in path)
        {
            _transform.LookAt(nextTail.transform);
            while (_transform.position != nextTail.transform.position)
            {
                _transform.position = Vector3.MoveTowards(_transform.position, nextTail.transform.position, _moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        _animator.SetBool("walk", false);

        //Занимаем клетку
        Target.Creature = Source;
        Target.TailType = Tail.TailTypes.Character;

        yield return new WaitForSeconds(1);

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
