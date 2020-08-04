using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilitySpriteType{
    none,
    single,
    aoe,
}
[CreateAssetMenu(menuName = "Ability/Support Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;

    public Formation usable;
    public Formation targetable;

    public virtual bool IsAttack { get { return false; } }
    public int metalCost;
    public int ammoCost;
    public bool isAOE;
    public bool isSelf;
    public bool cancelDisplay;
    public bool isRanged;
    public bool usableOutOfComabat;

    public AbilityParticleEffect actorParticles;

    public List<string> selfEffects;
    public List<string> targetEffects;
    public AbilitySpriteType spriteType;
    public AbilityParticleEffect targetPartcles;

    List<Effect> selfBuffs;
    public List<Effect> SelfBuffs {get { if (selfBuffs == null) Init(); return selfBuffs;}}
    List<Effect> targetBuffs;
    public List<Effect> TargetBuffs {get {if (targetBuffs == null) Init(); return targetBuffs;}}

    public AIPreferences ai;

    int targetStress;
    int selfStress;
    int targetHeal;
    int selfHeal;

    public virtual void Init()
    {
        selfBuffs = new List<Effect>();
        targetBuffs = new List<Effect>();
        foreach (string e in selfEffects)
        {
            selfBuffs.Add(EffectLibrary.GetEffect(e));
        }
        foreach (string e in targetEffects)
        {
            targetBuffs.Add(EffectLibrary.GetEffect(e));
        }
        targetStress = hasStress(true);
        selfStress = hasStress(false);
        targetHeal = hasDefense(true);
        selfHeal = hasDefense(false);
    }

    int hasDefense(bool target){
        if (target){
            foreach(Effect e in targetBuffs){
                if (e.IsHeal){
                    return e.GetAmount();
                }
            }
        } else {
            foreach(Effect e in selfBuffs){
                if (e.IsHeal){
                    return e.GetAmount();
                }
            }
        }
        return 0;
    }

    int hasStress(bool target){
        if (target){
            foreach(Effect e in targetBuffs){
                if (e.IsStress){
                    return e.GetAmount();
                }
            }
        } else {
            foreach(Effect e in selfBuffs){
                if (e.IsStress){
                    return e.GetAmount();
                }
            }
        }
        return 0;
    }

    public virtual void ApplyAbility(Unit actor, Unit target, ref AbilityResultList results)
    {
        if (cancelDisplay){
            results.display = false;
        }
        if (isAOE)
        {
            foreach(Unit u in target.UnitTeam.GetUnits())
            {
                if (u.targetedState == TargetedState.Targeted)
                {
                    foreach (Effect e in targetBuffs)
                    {
                        e.ApplyEffect(actor, u, ref results);
                    }
                }
            }
        } else
        {
            foreach(Effect e in targetBuffs)
            {
                e.ApplyEffect(actor, target, ref results);
            }
        }
        foreach (Effect e in selfBuffs)
        {
            e.ApplyEffect(actor, actor, ref results);
        }
    }

    public Team GetTargetTeam(Unit actor, Team ally, Team enemy)
    {
        if (IsAttack)
        {
            return actor.UnitTeam.isAlly == ally.isAlly ? enemy : ally;
        } else
        {
            return actor.UnitTeam.isAlly == ally.isAlly ? ally : enemy;
        }
    }

    public void SetTargets(Unit actor, Team ally, Team enemy)
    {
        
        if (IsAttack)
        {
            //attack abilities target enemies
            foreach (Unit u in actor.UnitTeam == ally ? enemy.GetUnits() : ally.GetUnits())
            {
                if (targetable.IsValidRank(actor, u)){
                    u.SetTargetState(TargetedState.Targeted);
                    u.SetToolTip(actor, this);
                    u.SetHealthChange(((Ability_Attack)this).DamageRange(actor, u) * -1);
                    u.SetStressChange(targetStress);
                }
                else {
                    u.SetTargetState(TargetedState.Untargeted);
                    u.SetToolTip(actor, null);
                    u.SetHealthChange(Vector2Int.zero);
                    u.SetStressChange(0);
                }
            }
            actor.SetHealthChange(new Vector2Int(selfHeal, selfHeal));
            actor.SetStressChange(selfStress);
        } else
        {
            //support abilities target allies
            foreach (Unit u in actor.UnitTeam == ally ? ally.GetUnits() : enemy.GetUnits())
            {
                if (u == actor){
                    continue;
                }
                if (u.stats.modifiers.CanBuff && targetable.IsValidRank(actor, u)){
                    u.SetTargetState(TargetedState.Targeted);
                    u.SetToolTip(actor, this);
                    u.SetHealthChange(new Vector2Int(targetHeal, targetHeal));
                    u.SetStressChange(targetStress);
                }
                else {
                    u.SetTargetState(TargetedState.Untargeted);
                    u.SetToolTip(actor, null);
                    u.SetHealthChange(Vector2Int.zero);
                    u.SetStressChange(0);
                }
            }
            if (isSelf)
            {
                //self abilities target self.
                actor.SetTargetState(TargetedState.Targeted);
                actor.SetToolTip(actor, this);
                if (usable.self){
                    actor.SetHealthChange(new Vector2Int(selfHeal, selfHeal));
                    actor.SetStressChange(selfStress);  
                } else {
                    actor.SetHealthChange(new Vector2Int(targetHeal, targetHeal));
                    actor.SetStressChange(targetStress);
                }
            }
        }
    }
}
