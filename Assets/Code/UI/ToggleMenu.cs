using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMenu : MonoBehaviour
{
    protected List<ToggleButton> toggles;
    
    void Awake(){
        toggles = new List<ToggleButton>(GetComponentsInChildren<ToggleButton>());
        foreach(ToggleButton tb in toggles){
            tb.group = this;
        }
    }

    void Start(){
         for (int i = 0; i < toggles.Count; ++i){
            if (i == 0){
                toggles[i].Select();
            }
            else {
                toggles[i].Deselect();
            }
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
        Debug.Log(menu);
        foreach(ToggleButton t in toggles){
            if (t.GetMenu() == menu){
                t.Select();
            } else {
                t.Deselect();
            }
        }
    }
}
