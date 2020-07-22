using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleSystem : MonoBehaviour
{
    public abstract void On(params GameObject[] parameters);
    public abstract void Execute();
    public abstract void Off();
}
