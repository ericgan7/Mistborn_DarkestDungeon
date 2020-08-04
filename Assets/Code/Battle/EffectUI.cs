using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectUI : MonoBehaviour
{
    [SerializeField] RectTransform rt;
    public Ability currentAbility;
    [SerializeField] TextMeshProUGUI unitName;
    [SerializeField] TextMeshProUGUI health;
    [SerializeField] TextMeshProUGUI effects;

    public void SetAbility(Unit actor, Ability ability, Unit target){
        currentAbility = ability;
        if (ability == null){
            unitName.text = "";
            health.text = "";
            effects.text = "";
            return;
        }
        int lines = 3;
        float width = 200f;
        bool on = gameObject.activeSelf;
        if (!on){
            gameObject.SetActive(true);
        }

        unitName.text = target.stats.GetName();
        width = Mathf.Max(LayoutUtility.GetPreferredWidth(unitName.rectTransform), width);
        Vector2Int hp = target.stats.Health();
        health.text = string.Format("<color={2}><b>Health</b></color>: {0}/{1}", hp.x, hp.y, ColorPallete.GetHexColor("Green"));
        effects.text = "";
        if (actor == target){
            foreach (Effect e in ability.SelfBuffs)
            {
                if (effects.text.Length == 0)
                {
                    effects.text = "Effects: \n";
                }
                effects.text += e.ToString();
                ++lines;
            }
            if (!ability.usable.self){
                foreach(Effect e in ability.TargetBuffs) { 
                    if (effects.text.Length == 0)
                    {
                        effects.text = "Effects: \n";
                    }
                    effects.text += e.ToString(target) + "\n";
                    ++lines;
                }
            }
        } else {
            if (ability.IsAttack){
                Ability_Attack aa = (Ability_Attack) ability;
                effects.text = string.Format("<color={1}><b>Accuracy</b></color>: {0}%\n", 
                    aa.accmod + actor.stats.GetStat(StatType.acc), ColorPallete.GetHexColor("Grey"));
                Vector2Int dmg = aa.DamageRange(actor, target);
                effects.text += string.Format("<color={2}><b>Damage</b></color>: {0} - {1}\n", dmg.x, dmg.y, ColorPallete.GetHexColor("Red"));
                int crit = aa.critmod + actor.stats.GetStat(StatType.crit);
                lines += 3;
                if (crit > 0){
                    effects.text += string.Format("<color={1}><b>Crit</b></color>: {0}%\n", crit, ColorPallete.GetHexColor("Yellow"));
                    ++lines;
                }
            }
            foreach(Effect e in ability.TargetBuffs) { 
                if (effects.text.Length == 0)
                {
                    effects.text = "Effects: \n";
                }
                effects.text += e.ToString(target) + "\n";
                ++lines;
            }
        }
        
        width = Mathf.Max(LayoutUtility.GetPreferredWidth(effects.rectTransform), width);
        rt.sizeDelta = new Vector2(width, lines * unitName.fontSize);
        if (!on){
            gameObject.SetActive(false);
        }
    }
}
