﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Attack Ability")]
public class Ability_Attack : Ability
{
    public override bool IsAttack => true;
    public int accmod = 0;
    public int critmod = 0;
    public float dmgmod = 0;
    const float countermod = 0.5f;

    public List<string> targetTraits;
    List<TraitTarget> traits;
    public List<TraitTarget> Traits {get {if (traits == null) Init(); return traits;}}

    public override void Init(){
        base.Init();
        traits = new List<TraitTarget>();
        foreach(string s in targetTraits){
            traits.Add(EffectLibrary.GetTraitTarget(s));
        }
    }

    public Result RollAccuracy(Unit actor, Unit target, int roll)
    {
        if (target.stats.Block())
        {
            return Result.Block;
        }
        int acc = actor.stats.GetStat(StatType.acc, target) + accmod;
        int dodge = target.stats.GetStat(StatType.dodge);
        int hitchance = Mathf.Clamp(acc - dodge,0 ,95);
        int grazechance = Mathf.Clamp(hitchance - dodge, 0, 95);
        if (roll > acc)
        {
            return Result.Miss;
        }
        else if (roll > hitchance)
        {
            return Result.Dodge;
        }
        else if (roll > grazechance)
        {
            return Result.Graze;
        }
        return RollCrit(actor, critmod);
    }

    public Result RollCrit(Unit actor, int mod)
    {
        int critroll = Random.Range(0, 100);
        int critchance = Mathf.Clamp(actor.stats.GetStat(StatType.crit) + mod, 0, 100);
        if (critroll > critchance)
        {
            return Result.Hit;
        }
        else
        {
            actor.stats.modifiers.UpdateOnAction(EffectType.crit);
            return Result.Crit;
        }
    }

    public override void ApplyAbility(Unit actor, Unit target, ref AbilityResultList results)
    {
        int roll = Random.Range(0, 100);
        if (isAOE)
        {
            foreach(Unit u in target.UnitTeam.GetUnits())
            {
                if (u.targetedState == TargetedState.Targeted)
                {
                    ApplyDamage(actor, u, roll, ref results);
                }
            }
        }else
        {
            ApplyDamage(actor, target, roll, ref results);
        }
        foreach (Effect e in SelfBuffs)
        {
            e.ApplyEffect(actor, actor, ref results);
        }
    }

    protected void ApplyDamage(Unit actor, Unit target, int roll, ref AbilityResultList results)
    {
        if (target.stats.modifiers.IsGuard){
            target = target.stats.modifiers.Guard.GetTarget();
        }
        Result r = RollAccuracy(actor, target, roll);
        if (r == Result.Miss || r == Result.Dodge || r == Result.Block)
        {
            //miss/block
            results.targets.Add(new AbilityResult()
            {
                actor = actor,
                target = target,
                amount = 0,
                result = r
            });
            //apply counter due to miss/block
            ApplyCounter(actor, target, ref results);
        }
        else
        {
            //apply damage
            float multiplier = r == Result.Crit ? 2 : 1;
            results.targets.Add(new AbilityResult()
            {
                actor = actor,
                target = target,
                amount = (int)(GetDamage(actor, target) * dmgmod * multiplier),
                result = r,
            });

            //apply debuffs
            foreach (Effect e in TargetBuffs)
            {
                e.ApplyEffect(actor, target, ref results);
            }
        }
        actor.stats.modifiers.UpdateOnAction(EffectType.attack, target);
    }

    public void ApplyCounter(Unit actor, Unit target, ref AbilityResultList results){
        Result counterResult = RollCrit(target, 0);
        float crit = counterResult == Result.Crit ? 1.5f: 1f;
        results.counter.Add(new AbilityResult()
        {
            actor = target,
            target = actor,
            amount = (int)(target.stats.GetStat(StatType.damage, actor) * countermod * crit),
            result = counterResult
        });
    }

    public virtual int GetDamage(Unit actor, Unit target){
        int damage = actor.stats.GetStat(StatType.damage, target);
        //ability trait targets
        foreach (TraitTarget t in Traits){
            damage += t.GetBonusDamage(target);
        }
        /*
        //character trait targets
        foreach (TraitTarget t in actor.stats.GetTraitTargets()){
            damage += t.GetBonusDamage(target);
        }
        */
        return damage;
    }

    public Vector2Int DamageRange(Unit actor, Unit target){
        Vector2Int damage = actor.stats.Damage();
        damage.x = Mathf.FloorToInt(damage.x * dmgmod);
        damage.y = Mathf.FloorToInt(damage.y * dmgmod);
        foreach (TraitTarget t in Traits){
            damage.x += t.GetBonusDamage(target);
            damage.y += t.GetBonusDamage(target);
        }
        /*
        //character trait targets
        foreach (TraitTarget t in actor.stats.GetTraitTargets()){
            damage.x += t.GetBonusDamage(target);
            damage.y += t.GetBonusDamage(target);
        }
        */
        return damage;
    }
}
