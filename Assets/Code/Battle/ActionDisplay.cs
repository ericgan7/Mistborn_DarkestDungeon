using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionDisplay : MonoBehaviour
{
    [SerializeField] RectTransform allyRanged;
    [SerializeField] RectTransform allyMelee;
    [SerializeField] RectTransform enemyRanged;
    [SerializeField] RectTransform enemyMelee;

    [SerializeField] List<RectTransform> allyTargets;
    [SerializeField] List<RectTransform> enemyTargets;

    [SerializeField] GameObject abilityTitleParent;
    [SerializeField] TextMeshProUGUI abilityTitle;
    
    List<Unit> moved;
    public Vector3 displayScale;
    HashSet<Result> healTypes;
    static Vector3 forward = new Vector3(150f, 0f, 0f);
    const float staggerDisplay = 0.5f;

    private void Awake()
    {
        healTypes = new HashSet<Result>() { Result.def, Result.heal};
        moved = new List<Unit>();
    }

    public void DisplayAbilityName(AbilityResultList results){
        abilityTitle.text = results.ability.abilityName;
        abilityTitleParent.gameObject.SetActive(true);
    }

    public void HideAbilityName() {
        abilityTitleParent.gameObject.SetActive(false);
    }

    public void ShowAbilityTargets(AbilityResultList results){
        results.actor.SetTargetState(TargetedState.Current);
        foreach (AbilityResult a in results.targets){
            a.target.SetTargetState(TargetedState.Targeted);
        }
    }

    public void DisplayActor(AbilityResultList results, float time){
        if (!results.display){
            return;
        }
        gameObject.SetActive(true);
        results.actor.transform.parent = transform;
        //check correct position to place in.
        if (results.actor.UnitTeam.isAlly){
            if (results.ability.IsAttack){
                results.actor.MoveUnit(results.ability.isRanged? 
                allyRanged.localPosition : allyMelee.localPosition, displayScale, time);
            }
            else {
                results.actor.MoveUnit(allyTargets[results.actor.Location].localPosition, displayScale, time);
            }
        } else {
            if (results.ability.IsAttack){
                results.actor.MoveUnit(results.ability.isRanged? 
                enemyRanged.localPosition : enemyMelee.localPosition, displayScale, time);
            }
            else {
                results.actor.MoveUnit(enemyTargets[results.actor.Location].localPosition, displayScale, time);
            }
            
        }
        results.actor.SetSprite(string.Format("{0}_{1}", results.actor.stats.GetClassName(), results.ability.abilityName));
        results.actor.TurnOffUIElemenets();
        moved.Add(results.actor);
        //add self abilities?
    }

    public void DisplayTargets(AbilityResultList results)
    {
        int direction = results.actor.UnitTeam.isAlly ? 1: -1;
        results.actor.UpdateUnitPosition(results.ability.isRanged? 
            (forward * -1 * direction) : forward * direction);
        int stagger = 0;
        foreach(AbilityResult r in results.targets)
        {
            r.target.transform.parent = transform;
            if (r.target.UnitTeam.isAlly)
            {
                r.target.MoveUnit(allyTargets[r.target.Location].localPosition, displayScale);
            }
            else
            {
                r.target.MoveUnit(enemyTargets[r.target.Location].localPosition, displayScale);
            }
            if (results.ability.IsAttack){
                r.target.SetSprite(string.Format("{0}_Hurt", r.target.stats.GetClassName()));
            }
            r.target.TurnOffUIElemenets();
            moved.Add(r.target);

            r.Display(stagger * staggerDisplay);
            ++stagger;
            //TODO check for death icons, etc.
        }
    }

    public void DisplayCounter(AbilityResultList results)
    {
        //results.counter.
        foreach(AbilityResult r in results.counter)
        {
            r.Display();
            r.actor.SetSprite(r.actor.stats.GetClassName() + "_Counter");
        }
    }
    
    public void HideAciton()
    {
        gameObject.SetActive(false);
        abilityTitleParent.SetActive(false);
        foreach(Unit u in moved)
        {
            u.transform.parent = u.UnitTeam.transform;
            u.SetSprite(u.stats.GetClassName() + "_Default", 0.75f);
            u.TurnOnUIElements();
            u.ResetPosition();
        }
        Reset();
    }

    public void Reset()
    {
        moved.Clear();
    }
}
