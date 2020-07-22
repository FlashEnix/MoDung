using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingleSystemManager : MonoBehaviour
{
    public static SingleSystemManager instance;
    private SingleSystem _currentSystem;
    private SingleSystem _defaultSystem;
    public List<SingleSystem> _allSystems;

    private IGameAction _linkAction;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        /*_defaultSystem = GetComponent<TailSystem>();*/
        _currentSystem = _defaultSystem;
        _allSystems = FindObjectsOfType<SingleSystem>().ToList();
    }
}
