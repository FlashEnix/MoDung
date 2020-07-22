using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputSystemManager : MonoBehaviour
{
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

        ClickObject.OnHoverIn += ClickObject_OnHoverIn;
        ClickObject.OnHoverOut += ClickObject_OnHoverOut;
        ClickObject.OnClick += ClickObject_OnClick;
        MatchSystem.instance.OnActionStart += MatchSystem_OnActionStart;
        MatchSystem.instance.OnActionEnd += MatchSystem_OnActionEnd;
        MatchSystem.instance.OnChangePlayer += MatchSystem_OnChangePlayer;

        EnableSystem(GetComponent<DefaultInputSystem>());
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

    private void ClickObject_OnClick(ClickObject obj)
    {
        if (_activeSystem != null)
        {
            _activeSystem.OnClick(obj);
            //EnableSystem(GetComponent<DefaultInputSystem>());
        }
    }

    private void ClickObject_OnHoverOut(ClickObject obj)
    {
        if (_activeSystem != null) _activeSystem.OnHoverOut(obj);
    }

    private void ClickObject_OnHoverIn(ClickObject obj)
    {
        if (_activeSystem != null) _activeSystem.OnHoverIn(obj);
    }

    public void EnableSystem(InputSystem system)
    {
        DisableSystem();
        _activeSystem = system;
        system.On();
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
        if (_activeSystem != null) _activeSystem.Off();
        _activeSystem = null;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
