using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI abilityName;
    [SerializeField] public Image usable;
    [SerializeField] public Image targets;
    [SerializeField] public TextMeshProUGUI abilityType;
    [SerializeField] public TextMeshProUGUI abilityStats;
    [SerializeField] public TextMeshProUGUI selfEffects;
    [SerializeField] public TextMeshProUGUI targetEffects;

    public void SetAbility(Ability a)
    {
        abilityName.text = a.abilityName;
        usable.sprite = a.usable.single;
        usable.color = Color.blue;
        targets.sprite = a.isAOE ? a.targetable.aoe : a.targetable.single;
        targets.color = a.IsAttack ? Color.red : Color.green;
        foreach (Effect e in a.selfBuffs)
        {
            if (selfEffects.text.Length == 0)
            {
                selfEffects.text = "Self: \n";
            }
            selfEffects.text += e.Stats();
        }
        foreach(Effect e in a.targetBuffs) { 
            {
                if (targetEffects.text.Length == 0)
                {
                    targetEffects.text = "Target: \n";
                }
                targetEffects.text += e.Stats();
            }
        }
        
    }

}
