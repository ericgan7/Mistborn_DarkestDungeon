using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardBox : MonoBehaviour
{
    public TextMeshProUGUI description;
    public ItemSlot slotPrefab;
    public ItemObj objPrefab;

    public Transform itemParent;

    public void SetText(string text){
        description.text = text;
    }
    public void AddReward(Reward reward){
        ItemSlot itemslot = Instantiate<ItemSlot>(slotPrefab, itemParent.transform);
        itemslot.temporary = true;
        itemslot.tempParent = this;
        ItemObj itemObj = Instantiate<ItemObj>(objPrefab, itemslot.transform);
        itemObj.UpdateItem(reward.GetItemRewards());
    }

    public void Close(){
        ItemSlot[] slots = itemParent.GetComponentsInChildren<ItemSlot>(false);
        foreach(ItemSlot s in slots){
            Destroy(s);
        }
        gameObject.SetActive(false);
    }

    public void CheckClose(){
        ItemSlot[] slots = itemParent.GetComponentsInChildren<ItemSlot>(false);
        int count = 0;
        foreach(ItemSlot s in slots){
            count ++;
        }
        if (count < 1){
            Close();
        }
    }

    public void Open(ref List<Reward> rewards){
        gameObject.SetActive(true);
        foreach(Reward r in rewards){
            if (r.IsItem){
                AddReward(r);
            }
        }
    }
}
