using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Ability Preference")]
public class EnemyAI : ScriptableObject
{
    public List<float> abilityPreferences;
    public static DecisionCompare compare = new DecisionCompare();

    public float GetAbilityPreference(int index)
    {
        if (index >= abilityPreferences.Count)
        {
            return 1f;
        }
        return abilityPreferences[index];
    }

    public Decision MakeDecision(Unit actor, Team ally, Team enemy, List<Ability> abilities)
    {
        List<Decision> decisions = new List<Decision>();
        for (int i = 0; i < abilities.Count; ++i)
        {
            if (abilities[i].IsAttack)
            {
                foreach (Unit u in ally.GetUnits())
                {
                    if (abilities[i].usable.IsValidRank(actor, actor) && abilities[i].targetable.IsValidRank(u, u)){
                        Decision d = new Decision()
                        {
                            desire = abilities[i].ai.GetDesire(u) * GetAbilityPreference(i),
                            ability = abilities[i],
                            target = u,
                        };
                        decisions.Add(d);
                    } else {
                        continue;
                    }
                }
            }
            else
            {
                foreach (Unit u in ally.GetUnits())
                {
                    Decision d = new Decision()
                    {
                        desire = abilities[i].ai.GetDesire(u) * GetAbilityPreference(i),
                        ability = abilities[i],
                        target = u,
                    };
                    decisions.Add(d);
                }
            }
        }
        decisions.Sort(compare);
        return decisions[0];
    }
}

public struct Decision
{
    public float desire;
    public Ability ability;
    public Unit target;
}

public class DecisionCompare: IComparer<Decision>
{
    public int Compare(Decision a, Decision b)
    {
        if (a.desire < b.desire)
        {
            return -1;
        } else if (a.desire == b.desire)
        {
            return 0;
        } else
        {
            return 1;
        }
    }
}
