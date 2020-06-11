using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ChoiceButton : MonoBehaviour
{
    public TextMeshProUGUI text;
    public EventManager eventManager;
    Choice selectedChoice;
    public void Init(Choice choice, EventManager manager){
        text.text = choice.text;
        eventManager = manager;
        selectedChoice = choice;
    }

    public void Select(){
        eventManager.SelectChoice(selectedChoice);
    }
}
