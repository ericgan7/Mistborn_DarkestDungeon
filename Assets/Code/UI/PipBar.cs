using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipBar : MonoBehaviour
{
    [SerializeField]RectTransform rt;
    public ProgressBarChange change;
    float max_x, max_y;
    protected Vector2Int values;

    public GameObject pipObj;
    public Sprite full;
    public Sprite empty;

    public float pipSpacing;
    public float pipWidth; // 7.5

    protected List<Image> pips;
    protected Vector2Int indicatorRange;

    [SerializeField] float cycleTime;
    protected float elapsed;
    protected bool flashing;
    protected bool on;

    public bool controlPipSize;

    public void Init(int current, int max){
        max_x = rt.sizeDelta.x;
        max_y = rt.sizeDelta.y;
        values = new Vector2Int(current, max);
        if (pips == null){
            pips = new List<Image>(values.y);
        }
        pipSpacing = max_x / values.y;
        if (pips.Count == values.y){
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
                if (controlPipSize){
                    pipRt.sizeDelta = new Vector2(pipWidth, rt.sizeDelta.y);
                }
                pipRt.anchoredPosition = new Vector2(pipSpacing * i, 0);
                pips.Add(p.GetComponent<Image>());
                pips[i].sprite = full;
            }
        }
    }

    public void SetIndicator(int amount){
        if (amount == 0){
            indicatorRange.x = values.y;
        }
        else if (amount > 0){
            indicatorRange = new Vector2Int(
                values.x,
                Mathf.Clamp(values.x + amount, 0, values.y)
            );
        }
        else {
            indicatorRange = new Vector2Int(
                Mathf.Clamp(values.x -1 + amount, 0, values.y),
                values.x
            );
            if (values.x - 1 < 0){ //if invalid range, set it to max so it will not display;
                indicatorRange.x = values.y;
            }
        }
    }

    public virtual void ShowIndicator(bool isOn){
        flashing = isOn;
        if (!isOn){
            for (int i = indicatorRange.x; i < indicatorRange.y; ++i){
                SetIndicatorSprite(i, isOn);
            }
        }
        elapsed = 0f;
        on = false;
    }

    public virtual void SetAmount(int amount){
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

    protected virtual void SetIndicatorSprite(int index, bool isOn){
        if (isOn){
            pips[index].sprite = index < values.x ? empty : full;
        } else {
            pips[index].sprite = index < values.x ? full : empty;
        }
    }

    public void FixedUpdate(){
        if (flashing){
            elapsed += Time.deltaTime;
            if (elapsed > cycleTime){
                on = !on;
                elapsed -= cycleTime;
                for (int i = indicatorRange.x; i < indicatorRange.y; ++i){
                    SetIndicatorSprite(i, on);
                }
            }
        }
    }
}
