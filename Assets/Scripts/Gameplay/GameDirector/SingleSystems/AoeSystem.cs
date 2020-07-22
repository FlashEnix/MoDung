using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


[Obsolete("Старая система")]
public class AoeSystem : MonoBehaviour
{
    private List<Tail> _radiusArray;

    void Start()
    {
        _radiusArray = new List<Tail>();
    }

    public void Run(params GameObject[] parameters)
    {
        ClearResult();
        AreaSelect(parameters[0].transform.position);
    }

    public void Off()
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
