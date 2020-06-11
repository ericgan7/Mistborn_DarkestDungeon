using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventBox : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public ChoiceButton prefab;

    public EventManager eventManager;
    public Transform parent;
    public VerticalLayoutGroup group;
    List<ChoiceButton> buttons;

    public void InitDialogue(Event dialogue, EventManager manager, Transform choiceParent){
        buttons = new List<ChoiceButton>();
        dialogueText.text = dialogue.text;
        eventManager = manager;
        parent = choiceParent;
        CreateChoices(dialogue);
        group.CalculateLayoutInputVertical();
        group.SetLayoutVertical();
    }
    
    void CreateChoices(Event dialogue){
        foreach(Choice choice in dialogue.choices){
            ChoiceButton b = Instantiate<ChoiceButton>(prefab, parent);
            b.Init(choice, eventManager);
            buttons.Add(b);
        }
    }

    public void ChooseButton(Choice choice){
        if (choice.Dialogue == null){
            return;
        }
        else {
            foreach(ChoiceButton b in buttons){
                Destroy(b.gameObject);
            }
            buttons.Clear();
        }
        group.CalculateLayoutInputVertical();
        group.SetLayoutVertical();
    }
}
