using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] RectTransform rt;
    [SerializeField] ProgressBarChange indicator;

    float max_x, max_y;
    Vector2 values;
    

    public void Init(int current, int max){
        max_x = rt.sizeDelta.x;
        max_y = rt.sizeDelta.y;
        values = new Vector2(current, max);
    }

    public void SetIndicator(int amount){
        indicator.SetMode(changeMode.flash);
        indicator.SetPosition(values.x / values.y * max_x, amount / values.y * max_x);
    }

    public void ShowIndicator(bool isOn){
        if (isOn){
            indicator.SetOn();
        } else {
            indicator.SetOff();
        }
    }

    public void SetAmount(int amount){
        values.x = amount;
        UpdateBar();
        if (indicator == null){
            return;
        }
        indicator.SetMode(changeMode.lerp);
        indicator.SetPosition(values.x / values.y * max_x, amount / values.y * max_x);
    }

    void UpdateBar(){
        rt.sizeDelta = new Vector2(
            values.x / values.y * max_x,
            max_y
        );
    }
}
