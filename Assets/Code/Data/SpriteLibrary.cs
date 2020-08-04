using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public static class SpriteLibrary
{
    static Dictionary<string, Sprite> characters;

    static Dictionary<string, Sprite> effects;

    static Dictionary<string, Sprite> abilities;

    static Dictionary<string, Sprite> backgrounds;

    static Dictionary<string, Sprite> portraits;

    static Dictionary<string, Sprite> abilityEffects;

    static bool inited = false;
    public static void Init(){
        Sprite[] sprites = Resources.LoadAll<Sprite>("Characters");
        characters = new Dictionary<string, Sprite>();
        foreach (Sprite s in sprites){
            characters.Add(s.name, s);
        }

        sprites = Resources.LoadAll<Sprite>("StatusEffects");
        effects = new Dictionary<string, Sprite>();
        foreach(Sprite s in sprites){
            effects.Add(s.name, s);
        }
        abilities = new Dictionary<string, Sprite>();
        sprites = Resources.LoadAll<Sprite>("Ability Icons");
        foreach(Sprite s in sprites){
            abilities.Add(s.name, s);
        }
        backgrounds = new Dictionary<string, Sprite>();
        sprites = Resources.LoadAll<Sprite>("Backgrounds");
        foreach(Sprite s in sprites){
            backgrounds.Add(s.name, s);
        }
        portraits = new Dictionary<string, Sprite>();
        sprites = Resources.LoadAll<Sprite>("Portraits");
        foreach(Sprite s in sprites){
            portraits.Add(s.name, s);
        }
        abilityEffects = new Dictionary<string, Sprite>();
        sprites = Resources.LoadAll<Sprite>("Effects");
        foreach(Sprite s in sprites){
            abilityEffects.Add(s.name, s);
        }
        inited = true;
    }
    public static Sprite GetCharSprite(string names)
    {
        if (!inited){
            Init();
        }
        if (characters.ContainsKey(names)){
            return characters[names];
        }
        else{
            Debug.Log("Cannot find " + names);
            string[] p = names.Split('_');
            if (characters.ContainsKey(string.Format("{0}_Default", p[0]))){
                return characters[string.Format("{0}_Default", p[0])];
            }
            else {
                return characters["Steel_Default"];
            }
        }
    }

    public static Sprite GetStatusSprite(string name){
        if (!inited){
            Init();
        }
        if (effects.ContainsKey(name)){
            return effects[name];
        }
        else {
            Debug.Log("Missing Sprite " + name);
            return null;
        }
    }

    public static Sprite GetAbilitySprite(string name){
        if (!inited){
            Init();
        }
        if (abilities.ContainsKey(name)){
            return abilities[name];
        }
        else {
            Debug.Log("Missing Sprite " + name);
            return null;
        }
    }

    public static Sprite GetBackground(string name){
        if (!inited){
            Init();
        }
        if (backgrounds.ContainsKey(name)){
            return backgrounds[name];
        } else {
            Debug.Log("Missing background "+ name);
            //TODO placeholder: find real way of organizing background sprites.
            int random = (int) Random.Range(0, backgrounds.Count);
            foreach (Sprite s in backgrounds.Values){
                if (random <= 0){
                    return s;
                }
                random--;
            }
            return null;
        }
    }

    public static Sprite GetPortrait(string name){
        if (portraits.ContainsKey(name)){
            return portraits[name];
        } else {
            return null;
        }
    }

    public static Sprite GetAbilityEffectSprite(string name){
        if (abilityEffects.ContainsKey(name)){
            return abilityEffects[name];
        } else {
            Debug.Log(name);
            return null;
        }
    }
}
