using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class EffectLibrary
{
    public static Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
    public static Dictionary<string, TraitTarget> targets = new Dictionary<string, TraitTarget>();

    public static Effect GetEffect(string param)
    {
        if (effects.ContainsKey(param))
        {
            return effects[param];
        }
        string[] p = param.Split(' ');
        switch (p[0])
        {
            case "Bleed":
                effects[param] = new Bleed(int.Parse(p[1]), int.Parse(p[2]));
                break;
            case "Counter":
                effects[param] = new Counter(float.Parse(p[1]), int.Parse(p[2]));
                break;
            case "Heal":
                effects[param] = new Heal(int.Parse(p[1]));
                break;
            case "Defend":
            case "Defense":
                effects[param] = new Defend(int.Parse(p[1]));
                break;
            case "Mark":
                effects[param] = new Mark(int.Parse(p[1]));
                break;
            case "Push":
            case "Pull":
                effects[param] = new PushPull(int.Parse(p[1]));
                break;
            case "Move":
                effects[param] = new Move();
                break;
            case "MoveSelf":
                effects[param] = new MoveSelf();
                break;
            case "Dodge":
            case "Damage":
            case "Crit":
            case "Acc":
            case "Speed":
            case "StressResist":
                effects[param] = new StatBuff(p[0], float.Parse(p[1]), int.Parse(p[2]));
                break;
            case "Block":
                effects[param] = new Block(int.Parse(p[1]), int.Parse(p[2]));
                break;
            case "Stun":
                effects[param] = new Stun();
                break;
            case "Suprise":
                effects[param] = new Suprise();
                break;
            case "Stress":
            case "StressHeal":
                effects[param] = new Stress(int.Parse(p[1]));
                break;
            case "Cleanse":
                effects[param] = new Cleanse();
                break;
            case "Guard":
                return new Guard(int.Parse(p[1]));
            case "Metal":
                return new Metal(int.Parse(p[1]));
            default:
                Debug.Log(param);
                break;
        }
        return effects[param];
    }

    public static TraitTarget GetTraitTarget(string param){
        if (targets.ContainsKey(param))
        {
            return targets[param];
        }
        string[] p = param.Split(' ');
        switch (p[0])
        {
            case "Ska":
                targets[param] = new SkaTarget(int.Parse(p[1]));
                break;
            case "Noble":
                targets[param] = new NobelTarget(int.Parse(p[1]));
                break;
            case "Stun":
                targets[param] = new StunTarget(int.Parse(p[1]));
                break;
            case "Bleed":
                targets[param] = new BleedTarget(int.Parse(p[1]));
                break;
            case "Mark":
                targets[param] = new MarkTarget(int.Parse(p[1]));
                break;
        }
        return targets[param];
    }

    public static Traits GetTraits(string param){
        switch(param){
            case "Ska":
                return new Ska();
            case "Noble":
                return new Noble();
            case "Steel":
                return new Momentum();
            case "Iron":
                return new Ironwall();
            case "Pewter":
                return new Constitution();
            case "Tin":
                return new Focus();
            case "Brass":
                return new Poise();
            case "Zinc":
                return new Inflame();
            default:
                Debug.Log("Missing Trait: " + param);
                return null;
        }
    }

    static int afflictions = 4;
    public static Affliction GenerateAffliction(int virtue){
        int roll = Random.Range(0, virtue);
        bool positive = false;
        if (roll < virtue){ //if we roll virtue, we get positive traits, else negative
            positive = true;
        }
        roll = Random.Range(0, afflictions);
        if (positive){
            switch (roll){
                case 0:
                    return new Affliction_Paranoid();
                case 1:
                    return new Affliction_Fearful();
                case 2:
                    return new Affliction_Abusive();
                case 3:
                    return new Affliction_Machoistic();
                default:
                    return new Affliction_Paranoid();
            }
        } else {
            switch(roll){
                case 0:
                    return new Affliction_Resolute();
                case 1:
                    return new Affliction_Righteous();
                case 2:
                    return new Affliction_Vengeful();
                case 3:
                    return new Affliction_Protective();
                default:
                    return new Affliction_Resolute();
            }
        }
    }
}

public class EffectParam
{
    public string key;
    public StatType statType;
    public float amount;
}
