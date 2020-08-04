using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPip : MonoBehaviour
{
    Image pip;
    Coroutine currentAnim;
    [SerializeField] float animTime;
    float velocity;
    float target;

    public void Awake(){
        pip = GetComponent<Image>();
        pip.fillAmount = 0f;
    }

    public void SetState(bool isOn){
        velocity = 0f;
        target = isOn ? 1f : 0f;
        if (currentAnim != null){
            StopCoroutine(currentAnim);
        }
        if (gameObject.activeInHierarchy){
            currentAnim = StartCoroutine(SetAmount());
        }
    }

    IEnumerator SetAmount(){
        float diff = Mathf.Abs(pip.fillAmount - target);
        while (diff > 0.001f){
            pip.fillAmount = Mathf.SmoothDamp(pip.fillAmount, target, ref velocity, animTime);
            diff = Mathf.Abs(pip.fillAmount - target);
            yield return new WaitForEndOfFrame();
        }
        currentAnim = null;
    }


}
