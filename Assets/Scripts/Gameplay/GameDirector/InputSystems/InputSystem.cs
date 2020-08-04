using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputSystem: MonoBehaviour
{
    public abstract void On();
    public abstract void Off();
    public abstract void OnHoverIn(Tail obj);
    public abstract void OnHoverOut(Tail obj);
    public abstract void OnClick(Tail obj);
}
