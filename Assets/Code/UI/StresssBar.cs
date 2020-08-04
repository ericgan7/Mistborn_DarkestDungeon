using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StresssBar : PipBar
{
    [SerializeField] Sprite panic;
    bool afflicted;

    public void SetAfflicted(bool afflict){
        afflicted = afflict;
    }

    protected override void SetIndicatorSprite(int index, bool isOn){
        if (afflicted){
            if (isOn){
                pips[index].sprite = index > values.x ? full : panic;
            } else {
                pips[index].sprite = index > values.x ? panic : full;
            }
        } else {
            base.SetIndicatorSprite(index, isOn);
        }
    }

    public override void ShowIndicator(bool isOn){
        if (afflicted){
            flashing = isOn;
            if (!isOn){
                for (int i = indicatorRange.x; i <= indicatorRange.y; ++i){
                    SetIndicatorSprite(i, isOn);
                }
            }
            elapsed = 0f;
            on = false;
        } else {
            base.ShowIndicator(isOn);
        }
    }

    public override void SetAmount(int amount){
        if (afflicted){
            values.x = amount;
            for (int i = 0; i < values.y; ++i){
                if (i < values.x){
                    pips[i].sprite = panic;
                } else {
                    pips[i].sprite = full;
                }
            }
        } else {
            base.SetAmount(amount);
        }
    }
}
