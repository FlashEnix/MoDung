using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryNotify : MonoBehaviour
{
    public Action OnCloseNotify;

    public Image ItemImage;
    public Text ItemName;
    private IInventoryItem _item;

    public void SetItem(IInventoryItem item)
    {
        _item = item;
        ItemName.text = item.ItemName;
        ItemImage.sprite = item.Icon;
    }

    public void CloseNotify()
    {
        OnCloseNotify?.Invoke();
        Destroy(gameObject);
    }
}
