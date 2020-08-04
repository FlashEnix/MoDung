using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tail : MonoBehaviour
{
    public enum TailTypes { Ground, Character, Treasure }

    public static event Action<Tail> OnHoverIn;
    public static event Action<Tail> OnHoverOut;
    public static event Action<Tail> OnClick;
    public static event Action<Tail> OnHovered;

    public TailTypes TailType = TailTypes.Ground;
    public CreatureStats Creature;
    public Treasure Treasure;

    private Material _material;
    
    void Start()
    {
        _material = GetComponent<Renderer>().material;
        Collider[] cols = Physics.OverlapSphere(transform.position, 0.2f);
        foreach (Collider col in cols)
        {
            if (Creature = col.gameObject.GetComponent<CreatureStats>())
            {
                TailType = TailTypes.Character;
            } else if (Treasure = col.gameObject.GetComponent<Treasure>())
            {
                TailType = TailTypes.Treasure;
            }
        }
    }

    private void OnMouseEnter()
    {
        OnHoverIn?.Invoke(this);   
    }

    private void OnMouseOver()
    {
        OnHovered?.Invoke(this);
        if (Input.GetMouseButtonUp(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            OnClick?.Invoke(this);
        }
    }

    private void OnMouseExit()
    {
        OnHoverOut?.Invoke(this);
    }

    public void SetColor(Color color)
    {
        _material.color = color;
    }
}
