using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectDisplay : MonoBehaviour
{
    [SerializeField] StatusIcon prefab;
    List<StatusIcon> icons;
    StatusEffectManager manager;
    
    public void Init(StatusEffectManager data){
        icons = new List<StatusIcon>();
        manager = data;
    }

    public void CheckSpawnIcon(EffectType type)
    {
        //TODO add traits icons
        foreach(StatusIcon si in icons)
        {
            if (si.type == type)//if icon exists, no need to spawn
            {
                //TODO add animation for updating status
                return;
            }
        }
        StatusIcon icon = Instantiate<StatusIcon>(prefab, transform);
        icon.manager = this.manager;
        icon.type = type;
        if (type == EffectType.baseClass){
            icon.SetImage(SpriteLibrary.GetStatusSprite(manager.unit.stats.GetClassName()));
        } else {
            icon.SetImage(SpriteLibrary.GetStatusSprite(EffectName.ToString(type)));
        }
        icon.SetPosition(icons.Count);
        icons.Add(icon);
    }

    public void CheckClearIcon()
    {
        for (int i = icons.Count -1; i >= 0; --i)
        {
            switch (icons[i].type)
            {
                case EffectType.bleed:
                    if (manager.Bleed.Count == 0)
                    {
                        RemoveIcon(i);
                    }
                    break;
                case EffectType.block:
                    if (manager.Block == null)
                    {
                        RemoveIcon(i);
                    }
                    break;
                case EffectType.buff:
                case EffectType.debuff:
                    bool remove = true;
                    foreach (StatusEffect s in manager.Modifiers)
                    {
                        if (s.Type == icons[i].type)
                        {
                            remove = false;
                        }
                    }
                    if (remove)
                    {
                        RemoveIcon(i);
                    }
                    break;
                case EffectType.mark:
                    if(manager.Mark == null)
                    {
                        RemoveIcon(i);
                    }
                    break;
                case EffectType.stun:
                    if (manager.Stun == null)
                    {
                        RemoveIcon(i);
                    }
                    break;
                case EffectType.guard:
                    if (manager.Guard == null){
                        RemoveIcon(i);
                    }
                    break;
                case EffectType.baseClass:
                    break;
                default:
                    Debug.Log("Missing clear icon switch");
                    break;
            }
        }
    }

    void RemoveIcon(int index)
    {
        Destroy(icons[index].gameObject);
        icons.RemoveAt(index);
    }
}
