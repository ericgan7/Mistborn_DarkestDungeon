//Textbox dialogue
    //this code handles resizing

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RectTransform background;
    VerticalLayoutGroup backgroundlayout;
    ContentSizeFitter backgroundSize;
    ContentSizeFitter textSize;
    
    public void Awake(){
        Init();
        gameObject.SetActive(false);
    }

    protected void Init(){
        backgroundlayout = background.GetComponent<VerticalLayoutGroup>();
        backgroundSize = background.GetComponent<ContentSizeFitter>();
        textSize = text.GetComponentInChildren<ContentSizeFitter>();
    }

    public void SetText(string message){
        text.text = message;
        SetSize();
    }

    void SetSize(){
        if (backgroundlayout == null) {
            Init();
        }
        Canvas.ForceUpdateCanvases();
        textSize.SetLayoutVertical();
        backgroundlayout.CalculateLayoutInputVertical();
        backgroundSize.SetLayoutVertical();
    }

    public float GetHeight(){
        return background.sizeDelta.y;
    }

    public virtual void Activate(DialogueManager manager){
        gameObject.SetActive(true);
    }
}
