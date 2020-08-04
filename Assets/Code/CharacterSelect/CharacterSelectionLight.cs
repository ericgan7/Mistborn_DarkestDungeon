using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class CharacterSelectionLight : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Light2D spotlight;
    Vector2 targetPosition;
    float velocity;
    [SerializeField] float moveTime;
    bool isMoving = false;

    public bool IsOn(){
        return spotlight.enabled;
    }

    public void SetOn(bool isOn){
        spotlight.enabled = isOn;
    }

    public void SetPositionImmediate(Vector2 newPosition){
        targetPosition = newPosition;
        rectTransform.anchoredPosition = new Vector2(newPosition.x, rectTransform.anchoredPosition.y);
    }

    public void SetPositionMoving(Vector2 newPosition){
        targetPosition =  newPosition;
        velocity = 0f;
        isMoving = true;
    }

    public void Update(){
        if (isMoving){
            rectTransform.anchoredPosition = new Vector2(
                Mathf.SmoothDamp(
                    rectTransform.anchoredPosition.x, targetPosition.x,
                    ref velocity, moveTime), 
                rectTransform.anchoredPosition.y);

            //only compares x coordinates. can add in y coordinates if necessary.
            if (Mathf.Abs(rectTransform.anchoredPosition.x - targetPosition.x) < 0.01){
                isMoving = false;
            }
        }
    }

    
}

