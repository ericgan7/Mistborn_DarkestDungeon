using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affliction_Abusive : Affliction
{
    public override string AfflictionName => "Abusive";

    int damageBoost = 1;
    public override float GetStat(StatType type, Unit actor = null, Unit target = null){
        if (type == StatType.damage){
            return damageBoost;
        }
        return 0f;
    }

    public override IEnumerator StressTeam(Unit actor){
        List<Unit> targets = actor.UnitTeam.GetUnits();
        targets.Remove(actor);
        int t = Mathf.FloorToInt(Random.Range(0, targets.Count));
        int damage = actor.stats.GetStat(StatType.damage);

        yield return new WaitForSeconds(0.5f);
        actor.SetSprite(string.Format("{0}_Counter", actor.stats.GetClassName()));
        Debug.Log(targets[t].stats.GetClassName());
        targets[t].stats.TakeDamage(damage);
        targets[t].SetSprite(string.Format("{0}_Hurt", targets[t].stats.GetClassName()));
        targets[t].CreatePopUpText(damage.ToString(), Color.red);
        yield return new WaitForSeconds(1.0f);

        actor.SetSprite(string.Format("{0}_Default", actor.stats.GetClassName()));
        targets[t].SetSprite(string.Format("{0}_Default", targets[t].stats.GetClassName()));
        yield return base.StressTeam(actor);
    }

    public override string ToString(){
        return string.Format("{0}: Damage +{1}, Chance to hurt teammates\n", AfflictionName, damageBoost);
    }
}
