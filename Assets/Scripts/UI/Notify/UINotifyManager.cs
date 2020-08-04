using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINotifyManager : MonoBehaviour
{
    public static UINotifyManager instance;

    public UINotifyNewItem NewItemNotifyPrefab;

    private Transform _canvas;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _canvas = UIDirector.instance.BaseCanvas.transform;
    }

    public void SendNotify(CreatureStats creature, IInventoryItem item, float seconds = 3)
    {
        UINotifyNewItem notify = Instantiate<UINotifyNewItem>(NewItemNotifyPrefab, _canvas);
        notify.Init(creature.avatar, item.Icon);
        GameHelper.instance.DelayMethod(() => Destroy(notify.gameObject), seconds);
    }
}
