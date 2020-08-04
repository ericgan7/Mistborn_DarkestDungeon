using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AfflictionDisplay : MonoBehaviour
{
    [SerializeField] Image unitImage;
    [SerializeField] TextMeshProUGUI affliction;
    [SerializeField] TextMeshProUGUI resolve;
    [SerializeField] GameObject resolveParent;
    [SerializeField] CameraShake shake;
    [SerializeField] float duration;
    [SerializeField] Vector3 startingScale;
    [SerializeField] Vector3 endingScale;
    [SerializeField] float shakeInensity;
    float elapsed;
    Vector3 velocity;

    [SerializeField] Sprite testAfflictionSprite;

    public float GetDuration(){
        return duration;
    }

    public void ShowResolve(Unit afflicted){
        resolve.text = string.Format("{0}'s resolve is tested..." , afflicted.stats.GetName());
        resolveParent.SetActive(true);
    }
    

    public void DisplayAffliction(Unit unit){
        resolveParent.SetActive(false);
        gameObject.SetActive(true);
        //unitImage.sprite = SpriteLibrary.GetCharSprite(unit.stats.GetClassName() + "_Affliction");
        unitImage.sprite = testAfflictionSprite;
        affliction.text = unit.stats.modifiers.Affliction.AfflictionName;
        unitImage.rectTransform.localScale = startingScale;
        velocity = Vector3.zero;
        elapsed = 0f;
        StartCoroutine(Display());
    }

    public IEnumerator Display(){
        shake.StartShake(shakeInensity, duration);
        while(elapsed < duration){
            elapsed += Time.deltaTime;
            unitImage.rectTransform.localScale = Vector3.SmoothDamp(unitImage.rectTransform.localScale, endingScale, ref velocity, duration);
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
    }
}
