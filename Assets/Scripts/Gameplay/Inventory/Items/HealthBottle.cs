using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HealthBottle : IInventoryItem, ITargetSource
{
    private IGameAction _action = new HealthAction(3);
    public string ItemName => "Зелье здоровья";
    public Sprite Icon => Resources.Load<Sprite>("Flame_Green");

    public IGameAction GameAction => _action;

    public int Count => 1;
}
