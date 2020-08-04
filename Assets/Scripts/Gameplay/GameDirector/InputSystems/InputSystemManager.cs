using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputSystemManager : MonoBehaviour
{
    private Tail _tailHovered;

    public static InputSystemManager instance;
    public event Action OnChangeSystem;

    private List<InputSystem> _inputSystems;
    private InputSystem _activeSystem;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _inputSystems = FindObjectsOfType<InputSystem>().ToList();

        Tail.OnHoverIn += ClickObject_OnHoverIn;
        Tail.OnHoverOut += ClickObject_OnHoverOut;
        Tail.OnClick += ClickObject_OnClick;
        Tail.OnHovered += Tail_OnHovered;
        MatchSystem.instance.OnActionStart += MatchSystem_OnActionStart;
        MatchSystem.instance.OnActionEnd += MatchSystem_OnActionEnd;
        MatchSystem.instance.OnChangePlayer += MatchSystem_OnChangePlayer;

        EnableSystem(GetComponent<DefaultInputSystem>());
    }

    private void Tail_OnHovered(Tail obj)
    {
        _tailHovered = obj;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            EnableSystem(GetComponent<DefaultInputSystem>());
        }
    }

    private void MatchSystem_OnChangePlayer(CreatureStats obj)
    {
        if (CheckInputPlayer(obj)) EnableSystem(GetComponent<DefaultInputSystem>());
    }

    private bool CheckInputPlayer(CreatureStats obj)
    {
        if (obj == null) obj = MatchSystem.instance.GetActivePlayer();

        AIScript ai = obj.GetComponent<AIScript>();

        if (ai != null && ai.isActiveAndEnabled)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void MatchSystem_OnActionEnd(IGameAction obj)
    {
        if (CheckInputPlayer(null)) EnableSystem(GetComponent<DefaultInputSystem>());
    }

    private void MatchSystem_OnActionStart(IGameAction obj)
    {
        DisableSystem();
    }

    private void ClickObject_OnClick(Tail obj)
    {
        if (_activeSystem != null)
        {
            _activeSystem.OnClick(obj);
            //EnableSystem(GetComponent<DefaultInputSystem>());
        }
    }

    private void ClickObject_OnHoverOut(Tail obj)
    {
        if (_activeSystem != null) _activeSystem.OnHoverOut(obj);
        _tailHovered = null;
    }

    private void ClickObject_OnHoverIn(Tail obj)
    {
        if (_activeSystem != null) _activeSystem.OnHoverIn(obj);
    }

    public void EnableSystem(InputSystem system)
    {
        DisableSystem();
        _activeSystem = system;
        system.On();
        if (_tailHovered) system.OnHoverIn(_tailHovered);
        OnChangeSystem?.Invoke();
    }

    public void EnableSystem(System.Type systemType)
    {
        foreach (InputSystem system in _inputSystems)
        {
            if (system.GetType() == systemType)
            {
                EnableSystem(system);
            }
        }
    }

    public void DisableSystem()
    {
        if (_activeSystem != null)
        {
            if (_tailHovered != null) _activeSystem.OnHoverOut(_tailHovered);
            _activeSystem.Off();
        }
            
        _activeSystem = null;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
