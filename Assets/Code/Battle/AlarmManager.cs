using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmManager : MonoBehaviour
{
    public int metalLevel;
    public int max_metal;
    public ProgressBar metalBar;
    //alarm;

    public void Start()
    {
        UpdateMetal(metalLevel);
    }

    public void UpdateMetal(int amount)
    {
        metalLevel = Mathf.Clamp(amount,0, max_metal);
        Debug.Log(metalBar.GetComponentInChildren<RectTransform>().sizeDelta);
        metalBar.UpdateBar(new Vector2Int(metalLevel, max_metal));
    }
    
}
