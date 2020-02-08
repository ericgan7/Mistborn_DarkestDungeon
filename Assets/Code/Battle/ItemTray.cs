using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTray : MonoBehaviour
{
    public Inventory inventory;

    public ItemSlot prefab;
    public List<ItemSlot> tray;
    public int inventorySize = 12;

    public void Awake()
    {
        RectTransform itemRt = prefab.GetComponent<RectTransform>();
        int height = (int)itemRt.sizeDelta.y;
        int width = (int)itemRt.sizeDelta.x;
        const int row_size = 6;
        tray = new List<ItemSlot>(inventorySize);
        for (int i = 0; i < inventorySize; ++i)
        {
            ItemSlot item = Instantiate<ItemSlot>(prefab, gameObject.transform);
            item.GetComponent<RectTransform>().anchoredPosition = 
                new Vector2(width * (i % row_size), -height * (i / row_size));
            item.inventory = this;
            tray.Add(item);
        }
        AssignItems();
    }

    public void AssignItems()
    {
        for (int i = 0; i < inventory.items.Count; ++i)
        {
            tray[i].AssignItem(inventory.items[i]);
        }
    }
}
