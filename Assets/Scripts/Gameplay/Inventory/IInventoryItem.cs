using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem
{
    int Count { get; }
    string ItemName { get; }
    Sprite Icon { get; }
    IGameAction GameAction { get; }
}
