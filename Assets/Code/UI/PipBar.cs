using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipBar : MonoBehaviour
{
    [SerializeField]RectTransform rt;
    public ProgressBarChange change;
    float max_x, max_y;
    Vector2Int values;

    public GameObject pipObj;
    public Sprite full;
    public Sprite indicator;
    public Sprite empty;

    public float pipSpacing;
    public float pipWidth; // 7.5

    List<Image> pips;
    Vector2Int indicatorRange;

    [SerializeField] float cycleTime;
    float elapsed;
    bool flashing;
    bool on;

    public void Init(int current, int max){
        max_x = rt.sizeDelta.x;
        max_y = rt.sizeDelta.y;
        values = new Vector2Int(current, max);
        if (pips == null){
            pips = new List<Image>(values.y);
        }
        pipSpacing = max_x / values.y;
        if (pips.Count == values.y){
            Debug.Log("Don't need to init new bars");
            return;
        }
        else if (pips.Count > values.y){
            for (int i = pips.Count - 1; i >= values.y; ++i){
                Destroy(pips[i]);
            }
        } else {
            for (int i = pips.Count; i < values.y; ++i){
                GameObject p = Instantiate(pipObj, transform);
                RectTransform pipRt = p.GetComponent<RectTransform>();
                //pipRt.sizeDelta = new Vector2(pipWidth, rt.sizeDelta.y);
                pipRt.anchoredPosition = new Vector2(pipSpacing * i, 0);
                pips.Add(p.GetComponent<Image>());
                pips[i].sprite = full;
            }
        }
    }

    public void SetIndicator(int amount){
        if (amount >= values.x){
            amount = values.x;
        }
        indicatorRange = new Vector2Int(values.x - (amount - 1), values.x);
    }

    public void ShowIndicator(bool isOn){
        flashing = isOn;
        if (!isOn){
            for (int i = indicatorRange.x; i <= indicatorRange.y; ++i){
                pips[i-1].sprite = full;
            }
        }
        elapsed = 0f;
        on = false;
    }

    public void SetAmount(int amount){
        values.x = amount;
        for (int i = 0; i < values.y; ++i){
            if (i < values.x){
                pips[i].sprite = full;
            } else {
                pips[i].sprite = empty;
            }
        }
        indicatorRange = Vector2Int.one;
        indicatorRange.y -= 1; //set indicator to invalid state;
    }

    public static float RoundToInterval(float fraction, int interval)
    {
        return (int)(fraction / (float)interval) * interval;
    }

    public void FixedUpdate(){
        if (flashing){
            elapsed += Time.deltaTime;
            if (elapsed > cycleTime){
                on = !on;
                elapsed -= cycleTime;
                if (on){
                    for (int i = indicatorRange.x; i <= indicatorRange.y; ++i){
                        pips[i-1].sprite = indicator;
                    }
                }
                else {
                    for (int i = indicatorRange.x; i <= indicatorRange.y; ++i){
                        pips[i-1].sprite = full;
                    }
                }
            }
        }
    }
}
