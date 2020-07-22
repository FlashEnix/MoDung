using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;

public class MatchSystem : MonoBehaviour
{
    public event Action<IGameAction> OnActionStart;
    public event Action<IGameAction> OnActionEnd;
    public event Action<CreatureStats> OnChangePlayer;
    public enum actionStatuses { start, end, cancel} 
    public static MatchSystem instance;

    public GameObject endPoint;
    
    public bool CanAction
    {
        get
        {
            return _currentAction == null;
        }
    }

    private int ActionsCount = 2;
    private List<CreatureStats> _allPlayers;
    [SerializeField] private CreatureStats _currentPlayer;
    [SerializeField] private int _pastActions = 0;
    [SerializeField] private IGameAction _currentAction;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        DamageSystem.instance.OnDeathPlayer += DamageSystem_OnDeathPlayer;
        _allPlayers = FindObjectsOfType<CreatureStats>().OrderBy(x=> x.team).ToList();
        _currentPlayer = _allPlayers[0];
        StatesExecute(_currentPlayer);
        OnChangePlayer?.Invoke(_currentPlayer);
    }

    public void AddPlayer(CreatureStats creature)
    {
        _allPlayers.Add(creature);
        UIDirector.instance.RefreshUIGameData();
    }

    private void DamageSystem_OnDeathPlayer(CreatureStats c, IDamage damage)
    {
        _allPlayers.Remove(c);
        if (_currentPlayer == c) ChangePlayer();

        //Заплатк адля конца
        if (_allPlayers.FirstOrDefault(x => x.team == 1) == null)
        {
            endPoint.SetActive(true);
        }
    }

    private void Update()
    {
        if (_currentAction != null)
        {
            if (_currentAction.status == actionStatuses.end)
            {
                EndAction(_currentAction);
            }
            else if (_currentAction.status == actionStatuses.cancel)
            {
                CancelAction(_currentAction);
            }
        }
    }

    public void RunAction(IGameAction action)
    {
        //if (UIDirector.instance.Notify != null) return;
        if (_currentAction != null) return;

        //Пропуск хода
        if (action == null)
        {
            EndAction(null);
            return;
        }

        if (action.Check())
        {
            OnActionStart?.Invoke(action);
            _currentAction = action;
            action.status = actionStatuses.start;
            StartCoroutine(action.Execute());
        }
        else
        {
            Debug.LogFormat("Действие {0} не прошло проверку", action.GetType());
        }
    }

    private void EndAction(IGameAction action)
    {
        _currentAction = null;
        if (++_pastActions >= ActionsCount)
        {
            ChangePlayer();
        }
        OnActionEnd?.Invoke(action);
    }

    private void CancelAction(IGameAction action)
    {
        _currentAction = null;
    }

    private void ChangePlayer()
    {
        int index = _allPlayers.FindIndex(x => x == _currentPlayer);

        if (index + 1 >= _allPlayers.Count)
        {
            _currentPlayer = _allPlayers[0];
        } else
        {
            _currentPlayer = _allPlayers[index + 1];
        }
        _pastActions = 0;
        StatesExecute(_currentPlayer);
        OnChangePlayer?.Invoke(_currentPlayer);
    }

    private void StatesExecute(CreatureStats creature)
    {
        ICharacterState[] states = creature.GetComponents<ICharacterState>();
        Debug.LogFormat("Нашли {0} состояний у {1}", states.Length, creature.creatureName);

        foreach (ICharacterState state in states)
        {
            state.Run();
        }
    }

    public CreatureStats GetActivePlayer()
    {
        return _currentPlayer;
    }

    public List<CreatureStats> GetAllPlayers()
    {
        return _allPlayers;
    }
}
