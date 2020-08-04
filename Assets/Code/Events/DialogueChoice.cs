using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DialogueChoice: MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI choiceText;
    protected DialogueManager manager;
    protected EventChoice current;
    VerticalLayoutGroup backgroundlayout;
    ContentSizeFitter backgroundSize;
    ContentSizeFitter textSize;

    public void Awake(){
        backgroundlayout = GetComponent<VerticalLayoutGroup>();
        backgroundSize = GetComponent<ContentSizeFitter>();
        textSize = choiceText.GetComponentInChildren<ContentSizeFitter>();
    }

    public void Init(EventChoice choice, DialogueManager parent){
        if (backgroundlayout == null){
            Awake();
        }
        manager = parent;
        current = choice;
        choiceText.text = choice.choiceDescription;
        Canvas.ForceUpdateCanvases();
        textSize.SetLayoutVertical();
        backgroundlayout.CalculateLayoutInputVertical();
        backgroundSize.SetLayoutVertical();
    }

    public virtual void OnPointerClick(PointerEventData pointer){
        manager.SelectChoice(current, null);
    }

    public void Activate(){
        gameObject.SetActive(true);
    }
}
