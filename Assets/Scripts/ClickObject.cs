using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickObject : MonoBehaviour
{
    public Texture2D Cursor;
    public enum types { creature, treasure, tail}
    private types _type;
    public bool isCreature { get => _type == types.creature; }
    public bool isTreasure { get => _type == types.treasure; }
    public bool isTail { get => _type == types.tail; }

    public static event Action<ClickObject> OnHoverIn;
    public static event Action<ClickObject> OnHoverOut;
    public static event Action<ClickObject> OnClick;

    private void Start()
    {
        if (GetComponent<CreatureStats>() != null) _type = types.creature;
        else if (GetComponent<Treasure>() != null) _type = types.treasure;
        else if (GetComponent<Tail>() != null) _type = types.tail;
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        OnHoverIn?.Invoke(this);
    }

    private void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonUp(0))
        {
            OnClick?.Invoke(this);
        }
    }

    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        OnHoverOut?.Invoke(this);
    }
}
