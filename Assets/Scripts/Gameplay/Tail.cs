using System;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public static event Action<Tail> OnHoverIn;
    public static event Action<Tail> OnHoverOut;
    public static event Action<Tail> OnClick;

    private Material _material;
    
    void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void OnMouseEnter()
    {
        OnHoverIn?.Invoke(this);   
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
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
