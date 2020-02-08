using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionIcon : MonoBehaviour
{
    public Sprite CurrentTurn;
    public Sprite Targeted;
    public Image image;

    private void Awake()
    {
        //image = GetComponent<Image>();
        //gameObject.SetActive(false);
    }

    public void SetTarget(TargetedState t)
    {
        switch (t)
        {
            case TargetedState.Current:
                image.sprite = CurrentTurn;
                image.color = Color.white;
                gameObject.SetActive(true);
                break;
            case TargetedState.Targeted:
                image.sprite = Targeted;
                image.color = Color.red;
                gameObject.SetActive(true);
                break;
            case TargetedState.Untargeted:
                gameObject.SetActive(false);
                break;
        }
    }

}
