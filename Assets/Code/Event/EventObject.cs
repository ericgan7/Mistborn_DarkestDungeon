using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventObject : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    Event current;
    EventManager manager;

    public void Awake(){
        manager = FindObjectOfType<EventManager>();
        gameObject.SetActive(false);
    }
    
    public void SetEvent(Event currentEvent){
        icon.sprite = currentEvent.dialogueImage;
        current = currentEvent;
        gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData pointer){
        manager.Open(current);
    }

}
