using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float duration;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 distance;
    Vector3 velocity = Vector3.zero;
    float alphaVelocity = 0f;
    float alpha = 0f;
    Vector3 target;
    RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        rt.localPosition = rt.localPosition + offset;
        target = rt.localPosition + distance;
        rt.localPosition = Vector3.SmoothDamp(rt.localPosition, target, ref velocity, duration);
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        rt.localPosition = Vector3.SmoothDamp(rt.localPosition, target, ref velocity, duration);
        text.color = new Color(1, 1, 1, Mathf.SmoothDamp(text.color.a, 0.5f, ref alphaVelocity, duration));
    }

    public void SetOffest(Vector3 destination) {
        rt.localPosition -= offset; //reset to original position
        offset = destination; 
        rt.localPosition += offset;
        target = rt.localPosition + distance;
    }

    public void SetMessage(string message) { text.text = message; }

    public void SetColor(Color messageColor) { text.faceColor = messageColor; }


}
