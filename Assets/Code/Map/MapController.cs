using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform map;
    Vector3 startPosition;
    Vector3 mapPosition;

    public void OnBeginDrag(PointerEventData p)
    {
        
    }

    public void OnDrag(PointerEventData p)
    {
        mapPosition = map.anchoredPosition;
        map.anchoredPosition += p.delta;
    }

    public void OnEndDrag(PointerEventData p)
    {

    }
}
