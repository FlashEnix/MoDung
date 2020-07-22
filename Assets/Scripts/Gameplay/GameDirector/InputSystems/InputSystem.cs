using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputSystem: MonoBehaviour
{
    public abstract void On();
    public abstract void Off();
    public abstract void OnHoverIn(ClickObject obj);
    public abstract void OnHoverOut(ClickObject obj);
    public abstract void OnClick(ClickObject obj);
}
