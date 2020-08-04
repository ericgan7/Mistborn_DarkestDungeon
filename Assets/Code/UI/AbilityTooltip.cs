using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityTooltip : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI abilityName;
    [SerializeField] public Image usable;
    [SerializeField] public Image targets;
    [SerializeField] public TextMeshProUGUI abilityStats;
    [SerializeField] public TextMeshProUGUI selfEffects;
    [SerializeField] public TextMeshProUGUI targetEffects;

    public void SetAbility(Ability a)
    {
        //reset
        abilityName.text = "";
        usable.sprite = null;
        targets.sprite = null;
        abilityStats.text = "";
        selfEffects.text = "";
        targetEffects.text = "";
        if (a == null){
            return;
        }
        abilityName.text = a.abilityName;
        usable.sprite = a.usable.single;
        usable.color = ColorPallete.GetColor("Highlight Blue");
        targets.sprite = a.isAOE ? a.targetable.aoe : a.targetable.single;
        targets.color = a.IsAttack ? ColorPallete.GetColor("Highlight Red") : ColorPallete.GetColor("Green");
        if (a.IsAttack){
            Ability_Attack aa = (Ability_Attack) a;
            abilityStats.text = string.Format("<color={2}><b>Accuracy</b></color>: {0}{1}%\n", 
                aa.accmod >= 0 ? "+" : "-", aa.accmod, ColorPallete.GetHexColor("Grey"));
            abilityStats.text += string.Format("<color={1}><b>Damage</b></color>: {0}%\n", 
                aa.dmgmod * 100, ColorPallete.GetHexColor("Red"));
            if (aa.critmod != 0){
                abilityStats.text += string.Format("<color={2}><b>Crit</b></color>: {0}{1}%\n", 
                    aa.critmod >= 0 ? "+" : "-", aa.critmod, ColorPallete.GetHexColor("Yellow"));
            }
        }
        foreach (Effect e in a.SelfBuffs)
        {
            if (selfEffects.text.Length == 0)
            {
                selfEffects.text = "Self: \n";
            }
            selfEffects.text += e.ToString();
        }
        foreach(Effect e in a.TargetBuffs) { 
            {
                if (targetEffects.text.Length == 0)
                {
                    targetEffects.text = "Target: \n";
                }
                targetEffects.text += e.ToString();
            }
        }
        
    }

}
