using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]protected RectTransform rt;
    protected float max;

    public virtual void Awake()
    {
        max = rt.sizeDelta.x;
    }

    public virtual void UpdateBar(Vector2Int amount)
    {
        rt.sizeDelta = new Vector2 ((float)amount.x / (float)amount.y * max, rt.sizeDelta.y);
        //spawn animation;
    }
}
