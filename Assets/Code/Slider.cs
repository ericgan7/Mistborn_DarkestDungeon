using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    RectTransform position;
    Vector2 target;
    Vector2 start;
    float elapsed;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        position = GetComponent<RectTransform>();
        elapsed = 0;
    }

    public void SetTarget(Vector2 t)
    {
        target = t;
        elapsed = 0;
        if (position == null)
        {
            position = GetComponent<RectTransform>();
        }
        start = position.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        position.anchoredPosition = Vector2.Lerp(start, target, speed * elapsed);
    }
}
