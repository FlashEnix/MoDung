using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
    }

    private void UISpell_OnClick(UISpell obj)
    {
    }

    public void Quit()
    {
        Application.Quit();
    }
}
