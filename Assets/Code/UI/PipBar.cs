using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipBar : ProgressBar
{
    public int numPips;
    public GameObject backpips;
    public Sprite on;
    public Sprite off;
    List<GameObject> pips;

    public override void Awake()
    {
        base.Awake();
        pips = new List<GameObject>(numPips);
        for (int i = 0; i < numPips; ++i)
        {
            GameObject p = Instantiate(backpips, transform);
            RectTransform pipRt = p.GetComponent<RectTransform>();
            pipRt.anchoredPosition = new Vector2(pipRt.sizeDelta.x * i, 0);
            pips.Add(p);
        }
    }

    public override void UpdateBar(Vector2Int amount)
    {
        //float f = (float)amount.x / (float)amount.y * max;
        ////make use of integer division to perfectly segemnt into num pips;
        //rt.sizeDelta = new Vector2(RoundInterval(f, numPips), rt.sizeDelta.y);
    }

    public static float RoundInterval(float fraction, int interval)
    {
        return (int)(fraction / (float)interval) * interval;
    }
}
