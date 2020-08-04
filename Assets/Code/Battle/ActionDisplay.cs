using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.Rendering.LWRP;

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

    [SerializeField] CameraShake shake;
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;

    [SerializeField] Image allyProjectile;
    [SerializeField] Image enemyProjectile;
    
    List<Unit> moved;
    public Vector3 displayScale;
    static Vector3 forward = new Vector3(200f, 0f, 0f);
    const float staggerDisplay = 0.5f;

    private void Awake()
    {
        moved = new List<Unit>();
    }

    public void DisplayAbilityName(AbilityResultList results){
        abilityTitle.text = results.ability.abilityName;
        abilityTitleParent.SetActive(true);
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
        results.actor.transform.SetParent(transform);
        results.actor.SetLight(Unit.UnitLight.attack, true);
        //set up effects sprites
        results.actor.SetEffectSprite(string.Format("{0}_{1}", results.actor.stats.GetClassName(), results.ability.abilityName));
        if (results.ability.actorParticles){
            results.actor.SetParticleEffect(results.ability.actorParticles);
        }
        //check correct position to place in.
        if (results.actor.UnitTeam.isAlly){
            if (results.ability.IsAttack){
                results.actor.MoveUnit(results.ability.isRanged? 
                allyRanged.localPosition : allyMelee.localPosition, displayScale, time);
                //results.actor.SetPosScale(results.ability.isRanged? allyRanged.localPosition : allyMelee.localPosition, displayScale);
            }
            else {
                results.actor.MoveUnit(allyTargets[results.actor.Location].localPosition, displayScale, time);
                //results.actor.SetPosScale(allyTargets[results.actor.Location].localPosition, displayScale);
            }
        } else {
            if (results.ability.IsAttack){
                results.actor.MoveUnit(results.ability.isRanged? 
                enemyRanged.localPosition : enemyMelee.localPosition, displayScale, time);
                //results.actor.SetPosScale(results.ability.isRanged? enemyRanged.localPosition : enemyMelee.localPosition, displayScale);
            }
            else {
                results.actor.MoveUnit(enemyTargets[results.actor.Location].localPosition, displayScale, time);
                //results.actor.SetPosScale(enemyTargets[results.actor.Location].localPosition, displayScale);
            }
        }
        results.actor.SetSprite(string.Format("{0}_{1}", results.actor.stats.GetSpriteHeader(), results.ability.abilityName));
        results.actor.TurnOffUIElemenets();
        //add self abilities?
    }

    public void DisplayTargets(AbilityResultList results, float duration)
    {
        int direction = results.actor.UnitTeam.isAlly ? 1: -1;
        results.actor.UpdateUnitPosition(results.ability.isRanged? (forward * -1 * direction) : forward * direction, duration);
        moved.Add(results.actor);
        bool hit = false;
        float crit = 1.0f;
        bool hasSprite = results.ability.spriteType == AbilitySpriteType.single ? true : false;
        if (results.ability.spriteType == AbilitySpriteType.aoe){
            hasSprite = false;
            if (results.targets.Count > 0){
                if (results.targets[0].target.UnitTeam.isAlly){
                    allyProjectile.sprite = SpriteLibrary.GetAbilityEffectSprite(results.ability.abilityName);
                    allyProjectile.enabled = true;
                } else {
                    enemyProjectile.sprite = SpriteLibrary.GetAbilityEffectSprite(results.ability.abilityName);
                    enemyProjectile.enabled = true;
                }
            }
        }
        foreach(AbilityResult r in results.targets)
        {
            r.target.transform.SetParent(transform);
            if (hasSprite){
                r.target.SetEffectSprite(results.ability.abilityName);
            }
            r.target.SetParticleEffect(results.ability.targetPartcles);
            
            if (r.target.UnitTeam.isAlly)
            {
                r.target.MoveUnit(allyTargets[r.target.Location].localPosition, displayScale);
            }
            else
            {
                r.target.MoveUnit(enemyTargets[r.target.Location].localPosition, displayScale);
            }
            if (results.ability.IsAttack){
                r.target.SetSprite(string.Format("{0}_Hurt", r.target.stats.GetSpriteHeader()));
                r.target.SetLight(Unit.UnitLight.hurt, true);
            }
            else {
                r.target.SetLight(Unit.UnitLight.attack, true);
            }
            r.target.TurnOffUIElemenets();
            moved.Add(r.target);
            if (r.result == Result.Hit || r.result == Result.Crit) {
                hit = true;
                if (r.result == Result.Crit){
                    crit = 2.0f;
                }
            }
            r.Display();
            //TODO check for death icons, etc.
        }
        enemyProjectile.transform.SetAsLastSibling();
        allyProjectile.transform.SetAsFirstSibling();
        if (hit){
            shake.StartShake(shakeIntensity * crit, shakeDuration);
        }
    }

    public void DisplayCounter(AbilityResultList results)
    {
        //results.counter.
        foreach(AbilityResult r in results.counter)
        {
            r.Display();
            r.actor.SetSprite(r.actor.stats.GetSpriteHeader() + "_Counter");
        }
    }
    
    public void HideAciton()
    {
        gameObject.SetActive(false);
        abilityTitleParent.SetActive(false);
        allyProjectile.enabled = false;
        enemyProjectile.enabled = false;
        foreach(Unit u in moved)
        {
            u.transform.SetParent(u.UnitTeam.transform);
            u.SetSprite(u.stats.GetSpriteHeader() + "_Default", 0.75f);
            u.SetEffectSprite(null);
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
