using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RewardSlot : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemCount;

    public void SetItem(ItemReward reward){
        itemImage.sprite = reward.item.icon;
        itemCount.text = reward.amount.ToString();
    }
}
