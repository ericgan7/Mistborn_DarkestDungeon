using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnText : MonoBehaviour
{
    [SerializeField] PopupText popupText;
    [SerializeField] GameObject unit;
    [SerializeField] float coolDown;
    Queue<PopupText> messageQueue;
    float elapsed;
    bool empty;

    public void Awake(){
        messageQueue = new Queue<PopupText>();
    }

    public bool IsEmpty(){
        return empty;
    }
    
    public void QueueMessage(string message, Color color){
        PopupText pop = Instantiate<PopupText>(popupText, unit.transform);
        pop.SetMessage(message);
        pop.SetColor(color);
        pop.SetOffest(new Vector3(Random.Range(-50f, 50f), 250f, 0f));
        messageQueue.Enqueue(pop);
        empty = false;
    }

    public void Update(){
        if (elapsed < coolDown){
            elapsed += Time.deltaTime;
        }
        else if (messageQueue.Count > 0){
            elapsed -= coolDown;
            PopupText pop = messageQueue.Dequeue();
            pop.ActivatePopUp();
        } {
            empty = true;
        }
    }
}
