using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Room : MonoBehaviour, IPointerClickHandler 
{
    public int id;
    public void OnPointerClick(PointerEventData p)
    {
        Debug.Log("click room");
    }
}
