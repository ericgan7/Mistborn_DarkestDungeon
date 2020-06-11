using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Support Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;

    public Formation usable;
    public Formation targetable;

    public virtual bool IsAttack { get { return false; } }
    public int metalCost;
    public bool isAOE;
    public bool isSelf;
    public bool cancelDisplay;
    public bool isRanged;
    public bool usableOutOfComabat;

    public List<string> selfEffects;
    public List<string> targetEffects;

    List<Effect> selfBuffs;
    public List<Effect> SelfBuffs {get { if (selfBuffs == null) Init(); return selfBuffs;}}
    List<Effect> targetBuffs;
    public List<Effect> TargetBuffs {get {if (targetBuffs == null) Init(); return targetBuffs;}}

    public AIPreferences ai;

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
    }

    public virtual void ApplyAbility(Team targets, Unit actor, Unit target, ref AbilityResultList results)
    {
        if (cancelDisplay){
            results.display = false;
        }
        if (isAOE)
        {
            foreach(Unit u in targets.GetUnits())
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
                    u.SetUIChange(((Ability_Attack)this).DamageRange(actor, u));
                }
                else {
                    u.SetTargetState(TargetedState.Untargeted);
                    u.SetToolTip(actor, null);
                    u.SetUIChange(Vector2Int.zero);
                }
            }
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
                }
                else {
                    u.SetTargetState(TargetedState.Untargeted);
                    u.SetToolTip(actor, null);
                }
            }
            if (isSelf)
            {
                //self abilities target self.
                actor.SetTargetState(TargetedState.Targeted);
                actor.SetToolTip(actor, this);
            }
        }
    }
}
