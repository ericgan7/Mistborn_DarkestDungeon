using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Ability", menuName = "Create Ability")]
public class OldAbility : ScriptableObject
{
    /*
    public string abilityName;
    public Sprite icon;

    public Formation targetable;
    public Formation usable;


    public bool IsAlly { get { return EffectParams.IsAlly(MainEffect.type); } }
    public bool IsAOE { get { return EffectParams.IsAOE(MainEffect.type); } }
    public bool isRanged;
    public bool isSelf;
    public virtual bool IsAttack { get { return MainEffect.IsDamage; } }

    public Effect MainEffect;
    public List<Effect> buffEffects;
    
    public List<Effect> GetEffects()
    {
        List<Effect> r = new List<Effect>(buffEffects);
        r.Add(MainEffect);
        return r;
    }

    public virtual void ApplyAbility(Team actorTeam, Team targets, Unit actor, ref AbilityResultList results)
    {

        foreach (Effect e in GetEffects())
        {
            switch (e.type)
            {
                case EffectType.self:
                    e.ApplyEffect(actor, actor, ref results);
                    break;
                case EffectType.ally:
                case EffectType.allyteam:
                    foreach (Unit a in actorTeam.GetUnits())
                    {
                        if (a.targetedState == TargetedState.Targeted) {
                            e.ApplyEffect(actor, a, ref results);
                            //a.stats.CheckCounter(this, actor, ref results);
                        }
                    }
                    break;
                case EffectType.enemy:
                case EffectType.enemyteam:
                    foreach (Unit a in targets.GetUnits())
                    {
                        if (a.targetedState == TargetedState.Targeted)
                        {
                            e.ApplyEffect(actor, a, ref results);
                            //a.stats.CheckCounter(this, actor, ref results);
                        }
                    }
                    break;
            }
        }
    }

    public void SetTargets(Unit actor, Team ally, Team enemy)
    {

        if (isSelf)
        {
            actor.SetTargetState(TargetedState.Targeted);
        }
        else if (IsAlly)
        {
            foreach (Unit t in ally.GetUnits())
            {
                t.SetTargetState(targetable.IsValidRank(actor, t)
                    ? TargetedState.Targeted : TargetedState.Untargeted);
            }
        }
        else
        {
            foreach (Unit t in enemy.GetUnits())
            {
                t.SetTargetState(targetable.IsValidRank(actor, t)
                    ? TargetedState.Targeted : TargetedState.Untargeted);
            }
        }
    }
    */
  
}
