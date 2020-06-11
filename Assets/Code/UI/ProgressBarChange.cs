using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum changeMode{
    flash,
    lerp,
    none
}

public class ProgressBarChange : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] RectTransform rt;
    [SerializeField] float cycleTime;
    float elapsed;
    changeMode mode;
    static Vector3 healDirection = new Vector3(-1, 1, 1);
    static Vector3 dmgDirection = Vector3.one;
    static Vector2 finalLerp;
    Vector2 velocity;
    [SerializeField] float lerpTime;

    void Awake(){
        bar = GetComponent<Image>();
        elapsed = 0f;
        rt = GetComponent<RectTransform>();
        mode = changeMode.none;
        bar.enabled = false;
        finalLerp = new Vector2(0f, rt.sizeDelta.y);
        rt.sizeDelta = finalLerp;
    }

    public void SetPosition(float start, float size){
        rt.anchoredPosition = new Vector2(start, rt.anchoredPosition.y);
        rt.sizeDelta = new Vector2(Mathf.Abs(size), rt.sizeDelta.y);
        if (size > 0){
            rt.localScale = healDirection;
        } else {
            rt.localScale = dmgDirection;
        }
        elapsed = 0f;
        velocity = Vector2.zero;
    }

    public void SetMode(changeMode newMode){
        mode = newMode;
    }

    public void SetOn(){
        bar.enabled = true;
    }

    public void SetOff(){
        mode = changeMode.none;
        bar.enabled = false;
    }

    public void FixedUpdate(){
        if (mode == changeMode.flash){
            elapsed += Time.deltaTime;
            if (elapsed > cycleTime){
                elapsed -= cycleTime;
                bar.enabled = !bar.enabled;
            }
        } else if (mode == changeMode.lerp){
            rt.sizeDelta = Vector2.SmoothDamp(rt.sizeDelta, finalLerp, ref velocity, lerpTime);
        }
    }
}
