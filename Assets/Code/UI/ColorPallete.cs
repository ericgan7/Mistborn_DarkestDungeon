using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorPallete
{
    static Dictionary<string, Color> pallete;

    private static void Init(){
        pallete = new Dictionary<string, Color>(){
            {"Black", new Color32(44, 62, 80, 255)},
            {"Teal", new Color32(22, 160, 133, 255)},
            {"Highlight Teal", new Color32(26, 188, 156, 255)},
            {"Green", new Color32(39, 174, 96, 255)},
            {"Hightlight Green", new Color32(46, 204, 113, 255)},
            {"Blue", new Color32(41, 128, 185, 255)},
            {"Highlight Blue", new Color32(52, 152, 219, 255)},
            {"Purple", new Color32(142, 68, 173, 255)},
            {"Highlight Purple", new Color32(155, 89, 182, 255)},
            {"Yellow", new Color32(243, 156, 18, 255)},
            {"Highlight Yellow", new Color32(241, 196, 15, 255)},
            {"Orange", new Color32(211, 84, 0, 255)},
            {"Highlight Orange", new Color32(230, 126, 34, 255)},
            {"Red", new Color32(192, 57, 43, 255)},
            {"Highlight Red", new Color32(231, 76, 60, 255)},
            {"White", new Color32(236, 240, 241, 255)},
            {"Grey", new Color32(149, 165, 166, 255)},
        };
    }

    public static Color GetColor(string color){
        if (pallete == null){
            Init();
        }
        if (pallete.ContainsKey(color)){
            return pallete[color];
        }
        Debug.Log("Missing color in pallete " + color);
        return pallete["Black"];
    }

    public static string GetHexColor(string color){
        Color c = GetColor(color);
        return string.Format("#{0}", ColorUtility.ToHtmlStringRGBA(c));
    }

    public static string GetStatHexColor(StatType type){
        switch(type){
            case StatType.health:
                return GetHexColor("Green");
            case StatType.defense:
                return GetHexColor("Highlight Blue");
            case StatType.will:
                return GetHexColor("Purple");
            case StatType.damage:
                return GetHexColor("Red");
            case StatType.crit:
                return GetHexColor("Yellow");
            case StatType.acc:
                return GetHexColor("White");
            case StatType.speed:
                return GetHexColor("Teal");
            case StatType.dodge:
                return GetHexColor("Blue");
            case StatType.bleedResist:
                return GetHexColor("Highlight Red");
            case StatType.moveResist:
                return GetHexColor("Highlight Teal");
            case StatType.stunResist:
                return GetHexColor("Highlight Yellow");
            case StatType.debuffResist:
                return GetHexColor("Orange");
            case StatType.stressResist:
                return GetHexColor("Highlight Purple");
            default:
                return GetHexColor("White");
        }
    }

    public static Color GetResultColor(Result results){
        switch (results){
            case Result.Hit:
                return GetColor("Red");
            case Result.Miss:
                return GetColor("Grey");
            case Result.Dodge:
                return GetColor("Grey");
            case Result.Graze:
                return GetColor("Orange");
            case Result.Heal:
                return GetColor("Green");
            case Result.Block:
                return GetColor("Blue");
            case Result.Buff:
                return GetColor("Teal");
            case Result.Crit:
                return GetColor("Yellow");
            case Result.DefCrit:
                return GetColor("Highlight Blue");
            case Result.Stress:
                return GetColor("Purple");
            case Result.StressHeal:
                return GetColor("White");
            default:
                return GetColor("Black");
        }
    }

    public static Color GetEffectColor(EffectType type){
        switch(type){
            case EffectType.buff:
                return GetColor("Highlight Blue");
            case EffectType.debuff:
                return GetColor("Highlight Orange");
            case EffectType.bleed:
                return GetColor("Highlight Red");
            case EffectType.stun:
                return GetColor("Highlight Yellow");
            case EffectType.mark:
                return GetColor("Red");
            case EffectType.block:
                return GetColor("Highlight Blue");
            case EffectType.move:
                return GetColor("Highlight Teal");
            //stress colors?
            default:
                return GetColor("Black");
        }
    }
}
