using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeInputSystem : InputSystem
{

    private List<Tail> _radiusArray;
    private AoeAction _activeSpell;
    public override void Off()
    {
        ClearResult();
    }

    public override void On()
    {
        _radiusArray = new List<Tail>();
        _activeSpell = (AoeAction)SpellSystem.instance.SelectedSpell;
    }

    public override void OnClick(Tail obj)
    {
        _activeSpell.Init(GameHelper.instance.GetTailFromObject(obj.gameObject));
        MatchSystem.instance.RunAction(_activeSpell);
    }

    public override void OnHoverIn(Tail obj)
    {
        ClearResult();
        AreaSelect(obj.transform.position);
    }

    public override void OnHoverOut(Tail obj)
    {
        ClearResult();
    }

    private void AreaSelect(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 1, 1 << 8);

        foreach (Collider col in colliders)
        {
            Tail colTail = col.gameObject.GetComponent<Tail>();
            _radiusArray.Add(colTail);
            colTail.SetColor(Color.red);
        }
    }

    private void ClearResult()
    {
        _radiusArray.ForEach(x => x.SetColor(Color.white));
        _radiusArray.Clear();
    }
}
