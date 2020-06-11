using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMenu : MonoBehaviour
{
    public List<ToggleButton> toggles;
    void Awake(){
        toggles = new List<ToggleButton>(GetComponentsInChildren<ToggleButton>());
    }
    void Start(){
         for (int i = 0; i < toggles.Count; ++i){
            if (i == 0){
                toggles[i].Select();
            }
            else {
                toggles[i].Deselect();
            }
            toggles[i].group = this;
        }
    }
    public void SelectToggle(ToggleButton tb){
        foreach (ToggleButton t in toggles){
            if (tb == t){
                tb.Select();
            } else {
                t.Deselect();
            }
        }
    }

    public void SelectToggle(string menu){
        foreach(ToggleButton t in toggles){
            if (t.menuName == menu){
                t.Select();
            } else {
                t.Deselect();
            }
        }
    }
}
