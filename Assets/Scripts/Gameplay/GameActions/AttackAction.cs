using System.Collections;
using System.Threading;
using UnityEngine;

public class AttackAction : TargetAction
{
    public GameObject ShootPrefab;

    private Transform _transform;
    private AnimEventController _animEvent;
    private bool _animEnd = false;

    private GameObject _shootModel;

    private new void Start()
    {
        base.Start();
        _transform = transform;
        _animEvent = GetComponentInChildren<AnimEventController>();
    }

    public override bool Check()
    {
        if (Source.team == Target.team) return false;
        if (Target.tag == "DeathPlayer") return false;
        float distance = Vector3.Distance(Source.transform.position, Target.transform.position);
        
        if (Source.rangeAttack == 0)
        {
            if (distance <= 1.6f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if ((int)distance > Source.rangeAttack)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public override IEnumerator Execute()
    {
        _transform.LookAt(Target.transform);
        _animator.SetTrigger("Attack");

        if (Source.rangeAttack == 0)
        {
            yield return MeleeAttack();
        } else
        {
            yield return RangeAttack();
        }
    }

    private IEnumerator MeleeAttack()
    {
        _animEvent.OnHitTime += _animEvent_OnHitTime;
        _animEvent.OnAnimationEnd += _animEvent_OnAnimationEnd;
        
        while(!_animEnd)
        {
            yield return new WaitForSeconds(0.5f);
        }

        _animEnd = false;

        status = MatchSystem.actionStatuses.end;
    }

    private void _animEvent_OnAnimationEnd()
    {
        _animEnd = true;
        _animEvent.OnAnimationEnd -= _animEvent_OnAnimationEnd;
    }

    private void _animEvent_OnHitTime()
    {
        int damage = Random.Range(Source.minATK, Source.maxATK);
        DamageSystem.instance.DealDamage(Source, Target, damage);
        _animEvent.OnHitTime -= _animEvent_OnHitTime;
    }

    private IEnumerator RangeAttack()
    {
        _animEvent.OnShootTime += _animEvent_OnShootTime;

        //Ждем пока выпустят снаряд
        while (_shootModel == null) yield return null;

        Vector3 targetPosition = new Vector3(Target.transform.position.x, _shootModel.transform.position.y, Target.transform.position.z);

        _shootModel.transform.LookAt(targetPosition);

        while (Vector3.Distance(_shootModel.transform.position, targetPosition) > 0.1f)
        {
            _shootModel.transform.position = Vector3.MoveTowards(_shootModel.transform.position, targetPosition, 3 * Time.deltaTime);
            yield return null;
        }

        Destroy(_shootModel);

        //Расчитываем урон
        int damage = Random.Range(Source.minATK, Source.maxATK);
        DamageSystem.instance.DealDamage(Source, Target,damage);

        yield return new WaitForSeconds(0.5f);
        status = MatchSystem.actionStatuses.end;

    }

    private void _animEvent_OnShootTime(Vector3 startPosition)
    {
        _shootModel = Instantiate(ShootPrefab, startPosition, Quaternion.identity);
        _animEvent.OnShootTime -= _animEvent_OnShootTime;
    }
}
