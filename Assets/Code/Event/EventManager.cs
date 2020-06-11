using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    GameState gameState;
    public Image eventImage;
    public EventLibrary library;
    //public DialogueBox prefab;
    public EventBox box;
    public VerticalLayoutGroup dialogueParent;
    public VerticalLayoutGroup choiceParent;
    EventBox currentDialogue;
    public RewardBox rewardBox;
    Choice priorChoice;

    //test
    public ItemTray items;

    void Awake(){
        library.Init();
        gameState = FindObjectOfType<GameState>();
        gameState.dm = this;
        gameObject.SetActive(false);
    }

    void Start(){
        //test start
        //Event start = library.GetDialogue(0);
        //DisplayDialogue(start);
    }

    public void EndCombat(){
        gameObject.SetActive(true);
        CheckRewards(priorChoice);
        DisplayDialogue(priorChoice.Dialogue);
    }
    public void Open(Event dialogue){
        gameObject.SetActive(true);
        // DialogueBox[] oldDialogues = dialogueParent.GetComponentsInChildren<DialogueBox>();
        // foreach (DialogueBox d in oldDialogues){
        //     Destroy(d.gameObject);
        // }
        DisplayDialogue(dialogue);
    }

    public void DisplayDialogue(Event d){
        if (d == null){
            return;
        }
        //Dialogue d = library.GetDialogue(id);
        eventImage.sprite = d.dialogueImage;
        //DialogueBox box = Instantiate<DialogueBox>(prefab, dialogueParent.transform);
        box.InitDialogue(d, this, choiceParent.transform);
        currentDialogue = box;
    }

    public void SelectChoice(Choice choice){
        priorChoice = choice;
        CheckRewards(choice);
        if (choice.IsEnd()){
            if (choice.startCombat){
                gameState.gc.StartCombat(null);
            } else if (choice.startMap){
                gameState.map.Open();
            }
            gameObject.SetActive(false);
        } else {
            currentDialogue.ChooseButton(choice);
            DisplayDialogue(choice.Dialogue);
            dialogueParent.CalculateLayoutInputVertical();
            dialogueParent.SetLayoutVertical();
        }
    }

    void CheckRewards(Choice choice){
        if (choice.RollSuccess(gameState.uic.CurrentCharacter, items.inventory)){
            List<Reward> rewards = choice.Reward;
            bool isItemOpen = false;
            foreach(Reward r in rewards){
                if (!isItemOpen &&r.IsItem){
                    isItemOpen = true;
                    rewardBox.Open(ref rewards);
                } else if (r.IsState){
                    r.GetStateRewards(gameState);
                } else {
                    r.GetCharacterRewards(gameState.uic.CurrentCharacter);
                }
            }
        }
        else {
            List<Reward> fail = choice.Failure;
            foreach(Reward r in fail){
                if (r.IsCharacter){
                    r.GetCharacterRewards(gameState.uic.CurrentCharacter);
                }
            }
        }
    }
}
