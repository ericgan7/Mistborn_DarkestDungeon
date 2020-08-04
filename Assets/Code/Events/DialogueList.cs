using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DialogueList : MonoBehaviour, IPointerClickHandler
{
    List<DialogueText> dialogues;
    [SerializeField] DialogueChoice useItem;
    List<DialogueChoice> choices;
    int activated;
    bool canContinue;
    [SerializeField] DialogueManager manager;

    [SerializeField] DialogueText dialoguePrefab;
    [SerializeField] DialogueChoice chiocePrefab;
    [SerializeField] DialogueAbilty abilityPrefab;
    [SerializeField] DialogueItem itemPrefab;
    [SerializeField] DialogueEnd endPrefab;

    [SerializeField] RectTransform content;
    VerticalLayoutGroup contentLayout;
    ContentSizeFitter contentSizeFitter;
    [SerializeField] RectTransform emptyBlock;
    [SerializeField] RectTransform viewport;
    [SerializeField] float emptyBlockSize;
    [SerializeField] float scrollTime;
    float velocity;

    [SerializeField] GameObject choiceBox;
    [SerializeField] RectTransform choiceContent;
    VerticalLayoutGroup choiceLayout;
    ContentSizeFitter choiceSizeFitter;

    [SerializeField] GameObject itemBox;
    [SerializeField] DialogueItemReward item;

    void Awake(){
        dialogues = new List<DialogueText>();
        choices = new List<DialogueChoice>();
        contentLayout = content.gameObject.GetComponent<VerticalLayoutGroup>();
        contentSizeFitter = content.gameObject.GetComponent<ContentSizeFitter>();
        choiceLayout = choiceContent.gameObject.GetComponent<VerticalLayoutGroup>();
        choiceSizeFitter = choiceContent.gameObject.GetComponent<ContentSizeFitter>();
        velocity = 0f;

        activated = 0;
        canContinue = true;
        itemBox.SetActive(false);
        choiceBox.SetActive(false);
    }

    public void CreateDialogue(string message){
        DialogueText t = Instantiate<DialogueText>(dialoguePrefab, content.transform);
        t.SetText(message);
        dialogues.Add(t);
    }

    public void CreateChoice(EventChoice e){
        if (e.itemRequirement != null){
            useItem.Init(e, manager);
        }
        DialogueChoice choice = Instantiate<DialogueChoice>(chiocePrefab, choiceContent.transform);
        choice.Init(e, manager);
        choices.Add(choice);
    }

    public void CreateReward(List<Item> reward){
        DialogueItem i = Instantiate<DialogueItem>(itemPrefab, content.transform);
        i.SetText("");
        dialogues.Add(i);
        item.AddItems(ItemLibrary.GenerateAmounts(reward, 500)); //placeholder value
        item.SetText("Yours for the taking...");
    }

    public void CreateAbility(Ability_Event ability){
        DialogueAbilty a = Instantiate<DialogueAbilty>(abilityPrefab, content.transform);
        a.SetText("");
        a.SetEvent(ability);
        dialogues.Add(a);
    }

    public void CreateEnd(){
        DialogueEnd e = Instantiate<DialogueEnd>(endPrefab, content.transform);
        e.SetText("");
        dialogues.Add(e);
    }
    
    public void ClearDialogue(){
        activated = 0;
        foreach(DialogueText t in dialogues){
            Destroy(t.gameObject);
        }
        dialogues.Clear();
        canContinue = true;
    }

    public void ClearChoices(){
        foreach(DialogueChoice c in choices){
            Destroy(c.gameObject);
        }
        choiceBox.SetActive(false);
        choices.Clear();
    }

    public void NextLine(){
        if (canContinue && activated < dialogues.Count){
            dialogues[activated].Activate(manager);
            //set the empty space to what is was -new text size. gradually grow to only show 1 message
            emptyBlock.sizeDelta = new Vector2(emptyBlock.sizeDelta.x, emptyBlock.sizeDelta.y - dialogues[activated].GetHeight());
            ++activated;
            emptyBlock.SetSiblingIndex(activated);
            Canvas.ForceUpdateCanvases();
            contentLayout.CalculateLayoutInputVertical();
            contentSizeFitter.SetLayoutVertical();
            choiceBox.SetActive(false);
        }  
        else if (itemBox.activeSelf == false){
            choiceBox.SetActive(true);
            Canvas.ForceUpdateCanvases();
            choiceLayout.CalculateLayoutInputVertical();
            choiceSizeFitter.SetLayoutVertical();
        }
    }

    public void OnPointerClick(PointerEventData pointer){
        NextLine();
    }

    //handles scrolling
    public void Update(){
        if (emptyBlockSize - emptyBlock.sizeDelta.y > 0.01){
            float y = Mathf.SmoothDamp(emptyBlock.sizeDelta.y, emptyBlockSize, ref velocity, scrollTime);
            emptyBlock.sizeDelta = new Vector2(emptyBlock.sizeDelta.x, y);
        } 
    }

    public void OpenItems(){
        itemBox.SetActive(true);
        canContinue = false;
    }

    public void CloseItems(){
        Debug.Log("Close Items called");
        itemBox.SetActive(false);
        canContinue = true;
    }
}
