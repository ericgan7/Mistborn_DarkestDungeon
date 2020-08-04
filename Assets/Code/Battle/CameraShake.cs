/*
    put on ui object to shake;
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class CameraShake : MonoBehaviour
{
    [SerializeField] RectTransform canvasTransform;
    Vector2 shakeTarget;
    float shakeIntensity;
    float shakeDuration;
    float elapsed;
    Vector2 velocity;
    Vector2 originalPosition;

    const float timeScale = 0.1f;

    public void StartShake(float intensity, float duration){
        shakeIntensity = intensity;
        shakeDuration = duration;
        elapsed = 0f;
        velocity = Vector2.zero;
        originalPosition = canvasTransform.anchoredPosition;
        StartCoroutine(Shake());
    }

    IEnumerator Shake(){
        float diff;
        float maxDiff;
        Vector2 target;
        while (elapsed < shakeDuration){
            elapsed += Time.deltaTime;
            maxDiff = 0f;
            //get a maximum shake;
            for (int i = 0; i < 5; ++i){
                target = new Vector2(
                    Random.Range(-shakeIntensity, shakeIntensity) + originalPosition.x,
                    Random.Range(-shakeIntensity, shakeIntensity) + originalPosition.y);
                diff = Mathf.Abs(canvasTransform.anchoredPosition.x - target.x) +
                        Mathf.Abs(canvasTransform.anchoredPosition.y - target.y);
                if (diff > maxDiff){
                    maxDiff = diff;
                    shakeTarget = target;
                }
            }
            canvasTransform.anchoredPosition = Vector2.SmoothDamp(
                canvasTransform.anchoredPosition, shakeTarget, ref velocity, timeScale
            );
            yield return new WaitForEndOfFrame();
        }
        canvasTransform.anchoredPosition = originalPosition;
    }
}
