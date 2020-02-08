using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Attack Ability")]
public class Ability_Attack : Ability
{
    public override bool IsAttack => true;
    public int accmod = 0;
    public int critmod = 0;
    public float dmgmod = 0;

    public Result RollAccuracy(Unit actor, Unit target)
    {
        int acc = actor.stats.Acc() + accmod;
        int dodge = target.stats.Dodge();
        int critchance = Mathf.Clamp(actor.stats.Crit() + critmod, 0, 100);
        int hitchance = Mathf.Clamp(acc - dodge,0 ,95);
        int grazechance = Mathf.Clamp(hitchance - dodge, 0, 95);
        int roll = Random.Range(0, 100);
        int critroll = Random.Range(0, 100);
        if (roll > acc)
        {
            return Result.miss;
        }
        else if (roll > hitchance)
        {
            return Result.dodge;
        }
        else if (roll > grazechance)
        {
            return Result.graze;
        }
        else if (critroll > critchance)
        {
            return Result.hit;
        } else
        {
            return Result.crit;
        }
    }

    public override void ApplyAbility(Team targets, Unit actor, Unit target, ref AbilityResultList results)
    {
        if (isAOE)
        {
            foreach(Unit u in targets.GetUnits())
            {
                if (u.targetedState == TargetedState.Targeted)
                {
                    Apply(actor, u, ref results);
                }
            }
        }else
        {
            Apply(actor, target, ref results);
        }
        foreach (Effect e in selfBuffs)
        {
            e.ApplyEffect(actor, actor, ref results);
        }
    }

    private void Apply(Unit actor, Unit target, ref AbilityResultList results)
    {
        //TODO: Check for counter
        Result r = RollAccuracy(actor, target);
        if (r == Result.miss || r == Result.dodge)
        {
            //miss/dodge
            results.targets.Add(new AbilityResult()
            {
                target = target,
                amount = 0,
                result = r
            });
        }
        else
        {
            //apply damage
            int multiplier = r == Result.crit ? 2 : 1;
            results.targets.Add(new AbilityResult()
            {
                target = target,
                amount = (int)(actor.stats.Damage() * dmgmod * multiplier),
                result = r,
            });

            //apply debuffs
            foreach (Effect e in targetBuffs)
            {
                e.ApplyEffect(actor, target, ref results);
            }
        }
    }
}
