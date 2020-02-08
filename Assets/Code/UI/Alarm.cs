using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Alarm : MonoBehaviour, IPointerClickHandler
{
    public Image radial;
    public TextMeshProUGUI number;
    public Animator cycleAnim;

    public int alarmLevel;

    public List<float> fillLevels;
    public float speed;
    float velocity;
    public int currentRadial;

    public void Awake()
    {
        radial.fillAmount = 0;
        Debug.Log(radial.fillAmount);
        currentRadial = 0;
        number.text = alarmLevel.ToString();
        cycleAnim = GetComponent<Animator>();
        cycleAnim.enabled = true;
    }

    public void SetAlarmLevel(int change)
    {
        StartCoroutine(ChangeAlarm(change));
    }

    public IEnumerator ChangeAlarm(int change)
    {
        velocity = 0;
        while (change > 0)
        {
            ++currentRadial;
            Debug.Log(radial);
            --change;
            if (currentRadial == fillLevels.Count)
            {
                cycleAnim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                cycleAnim.SetTrigger("cycle");
                //yield return StartCoroutine(WaitforAnimation(cycleAnim));
                yield return new WaitForSeconds(3.0f);
                currentRadial = 0;
                alarmLevel += 1;
                number.text = alarmLevel.ToString();
                cycleAnim.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
                cycleAnim.ResetTrigger("cycle");
            }
            else
            {
                float diff = fillLevels[currentRadial] - radial.fillAmount;
                while (diff > 0.001)
                {
                    radial.fillAmount = Mathf.SmoothDamp(radial.fillAmount,
                        fillLevels[currentRadial],
                        ref velocity,
                        speed
                        );
                    diff = fillLevels[currentRadial] - radial.fillAmount;
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }

    public IEnumerator WaitforAnimation(Animation animation)
    {
        while (animation.isPlaying)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

    public void OnPointerClick(PointerEventData p)
    {
        Debug.Log("Alarm");
        SetAlarmLevel(1);
        //++currentRadial;
        //cycleAnim.SetTrigger("increment");
        //cycleAnim.ResetTrigger("cycle");
    }

}
