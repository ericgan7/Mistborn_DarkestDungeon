using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Support Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public Sprite icon;

    public Formation targetable;
    public Formation usable;

    public virtual bool IsAttack { get { return false; } }
    public bool isAOE;
    public bool isSelf;

    public List<string> selfEffects;
    public List<string> targetEffects;

    public List<Effect> selfBuffs;
    public List<Effect> targetBuffs;

    public void Init()
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
        if (isSelf)
        {
            //self abilities target self.
            actor.SetTargetState(TargetedState.Targeted);
        } else if (IsAttack)
        {
            //attack abilities target enemies
            foreach (Unit u in actor.UnitTeam == ally ? enemy.GetUnits() : ally.GetUnits())
            {
                u.SetTargetState(targetable.IsValidRank(actor, u) ? 
                    TargetedState.Targeted :TargetedState.Untargeted);
            }
        } else
        {
            //support abilities target allies
            foreach (Unit u in actor.UnitTeam == ally ? ally.GetUnits() : enemy.GetUnits())
            {
                u.SetTargetState(targetable.IsValidRank(actor, u) ?
                    TargetedState.Targeted : TargetedState.Untargeted);
            }
        }
    }
}
