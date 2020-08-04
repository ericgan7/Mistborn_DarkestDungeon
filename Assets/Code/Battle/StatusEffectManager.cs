using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager: MonoBehaviour
{
    public StatusIcon prefab;
    public List<StatusIcon> icons;
    [SerializeField] Unit unit;
    GameState state;

    List<Traits> traits;
    List<StatusEffect> modifiers;
    List<StatusEffect> bleed;
    List<StatusEffect> terror;
    StatusEffect stun;
    StatusEffect mark;
    Block block;
    Guard guard;

    public bool IsStunned {get { return stun != null; } }
    public bool IsMarked { get { return mark != null; } }
    public bool IsBlock { get { return block != null; } }
    public bool IsGuard { get { return guard != null; } }
    public bool IsBleed { get { return bleed.Count > 0; }}
    public bool CanBuff { get { return Affliction == null || Affliction.CanBuff; }}
    public List<StatusEffect> Modifiers { get { return modifiers; } }
    public List<StatusEffect> Bleed { get { return bleed; } }
    public List<StatusEffect> Terror { get { return terror; } }
    public List<Traits> Traits { get {return traits; } }
    public StatusEffect Stun { get { return stun; } }
    public StatusEffect Mark { get { return mark; } }
    public Block Block { get { return block; } }
    public Guard Guard { get { return guard; } }
    public Affliction Affliction { get; set; }

    public void Awake()
    {
        if (traits == null){
            Init();
        }
    }
    
    void Init(){
        icons = new List<StatusIcon>();
        modifiers = new List<StatusEffect>();
        bleed = new List<StatusEffect>();
        traits = new List<Traits>();
        terror = new List<StatusEffect>();
        state = FindObjectOfType<GameState>();
        //initialize traits from unit character
        Affliction = null;
    }

    public void SetCharacter(Character c){
        if (traits == null){
            Init();
        }
        traits.Clear();
        traits.AddRange(c.traits);
        foreach(Traits t in traits){
            if (t.Type == EffectType.baseClass){
                CheckSpawnIcon(t.Type);
            }
        }
    }

    public void OnTurnBegin()
    {
        for (int i = modifiers.Count - 1; i >= 0; --i)
        {
            if (modifiers[i].DecreaseDuration())
            {
                modifiers.RemoveAt(i);
            }
        }
        for (int i = bleed.Count - 1; i >= 0; --i)
        {
            if (bleed[i].DecreaseDuration())
            {
                bleed.RemoveAt(i);
            }
        }
        for (int i = terror.Count - 1; i >= 0; --i){
            if (terror[i].DecreaseDuration()){
                terror.RemoveAt(i);
            }
        }
        if (IsMarked)
        {
            if (mark.DecreaseDuration())
            {
                mark = null;
            }
        }
        if (IsBlock)
        {
            if (block.DecreaseDuration())
            {
                block = null;
            }
        }
        if (IsGuard){
            if (guard.DecreaseDuration()){
                guard = null;
            }
        }
        foreach(Traits t in traits){
            t.ApplyOverTime(unit);
        }
        CheckClearIcon();
    }

    public void AddEffect(StatusEffect effect)
    {
        switch (effect.Type)
        {
            case EffectType.bleed:
                bleed.Add(effect);
                break;
            case EffectType.debuff:
            case EffectType.buff:
                modifiers.Add(effect);
                break;
            case EffectType.mark:
                if (!IsMarked)
                mark = effect;
                break;
            case EffectType.block:
                block = effect as Block;
                break;
            case EffectType.stun:
                stun = effect;
                break;
            case EffectType.guard:
                guard = effect as Guard;
                break;

        }
        CheckSpawnIcon(effect.Type);
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
        icon.manager = this;
        icon.type = type;
        if (type == EffectType.baseClass){
            icon.SetImage(SpriteLibrary.GetStatusSprite(unit.stats.GetClassName()));
        } else {
            icon.SetImage(SpriteLibrary.GetStatusSprite(EffectName.ToString(type)));
        }
        icon.SetPosition(icons.Count);
        icons.Add(icon);
    }

    //TO DO spawn affliction icon

    public void CheckClearIcon()
    {
        for (int i = icons.Count -1; i >= 0; --i)
        {
            switch (icons[i].type)
            {
                case EffectType.bleed:
                    if (bleed.Count == 0)
                    {
                        RemoveIcon(i);
                    }
                    break;
                case EffectType.block:
                    if (block == null)
                    {
                        RemoveIcon(i);
                    }
                    break;
                case EffectType.buff:
                case EffectType.debuff:
                    bool remove = true;
                    foreach (StatusEffect s in modifiers)
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
                    if(mark == null)
                    {
                        RemoveIcon(i);
                    }
                    break;
                case EffectType.stun:
                    if (stun == null)
                    {
                        RemoveIcon(i);
                    }
                    break;
                case EffectType.guard:
                    if (guard == null){
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

    public void RemoveIcon(int index)
    {
        Destroy(icons[index].gameObject);
        icons.RemoveAt(index);
    }

    public float GetStatModifier(StatType type, Unit target = null)
    {
        float amount = 0;
        if (unit.UnitTeam == null){
            return 0; //curios and events have no team;
        }
        foreach(StatusEffect e in modifiers)
        {
            amount += e.GetStat(type);
        }
        foreach (StatusEffect e in state.metal.GetMetalEffects().GetEffects(unit.UnitTeam)){
            amount += e.GetStat(type);
        }
        foreach (StatusEffect e in state.am.GetAlarmEffects().GetEffects(unit.UnitTeam)){
            amount += e.GetStat(type);
        }
        foreach(EquipableItem item in unit.stats.GetItems()){
            if (item == null){
                continue;
            }
            foreach(StatusEffect e in item.GetStats()){
                amount += e.GetStat(type);
            }
        }
        foreach(Traits t in traits){
            amount += t.GetStat(type, unit, target);
        }
        if (Affliction != null){
            amount += Affliction.GetStat(type);
        }
        return amount;
    }

    public void ClearStun()
    {
        stun = null;
        CheckClearIcon();
    }

    public void BlockAttack()
    {
        if (block.DecreaseAmount())
        {
            block = null;
            CheckClearIcon();
        }
    }

    public void ClearAllEffects(){
        //does not clear bleed
        terror.Clear();
        stun = null;
        mark = null;
        for (int i = modifiers.Count - 1; i >= 0; --i){
            if (modifiers[i].IsStatChange && modifiers[i].IsDebuff){
                modifiers.RemoveAt(i);
            }
        }
    }

    public void ShowUI(bool isOn){
        gameObject.SetActive(isOn);
    }

    public void UpdateOnAction(EffectType type, Unit target = null){
        switch(type){
            case EffectType.move:
                foreach (Traits t in traits){
                    t.OnMove(unit);
                }
                break;
            case EffectType.attack:
                foreach (Traits t in traits){
                    t.OnAttack(unit, target);
                }
                break;
            case EffectType.crit:
                foreach(Traits t in traits){
                    t.OnCrit();
                }
                break;
        }
    }

    public void AttackModifiers(Ability_Attack a, Unit actor, Unit target, ref AbilityResultList results){
        foreach(Traits t in traits){
            t.ModifyAttack(a, actor, target, ref results);
        }
    }

    public bool HasEffect(EffectType type){
        switch(type){
            case EffectType.bleed:
                return bleed.Count > 0;
            case EffectType.debuff:
                foreach (StatusEffect s in modifiers)
                {
                    if (s.Type == EffectType.debuff){
                        return true;
                    }
                }
                return false;
            default:
                return true;
        }
    }

}
