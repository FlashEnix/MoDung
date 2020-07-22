using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryItemScript : MonoBehaviour, IPointerClickHandler
{
    IInventoryItem _item;
    Image _image;

    public void Awake()
    {
        _image = GetComponent<Image>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        InventorySystem.instance.SelectItem(_item);
    }

    public void SetItem(IInventoryItem item)
    {
        _item = item;
        _image.sprite = item.Icon;
    }
}
