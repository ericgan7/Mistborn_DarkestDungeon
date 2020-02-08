using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionDisplay : MonoBehaviour
{
    [SerializeField] RectTransform allyRanged;
    [SerializeField] RectTransform allyMelee;
    [SerializeField] RectTransform enemyRanged;
    [SerializeField] RectTransform enemyMelee;

    [SerializeField] List<RectTransform> allyTargets;
    [SerializeField] List<RectTransform> enemyTargets;

    List<Unit> moved;
    public Vector3 displayScale;
    HashSet<Result> healTypes;

    private void Awake()
    {
        healTypes = new HashSet<Result>() { Result.def, Result.heal};
        moved = new List<Unit>();
    }

    public void DisplayAction(AbilityResultList results)
    {
        gameObject.SetActive(true);
        //check correct position to place in.
        results.actor.SetPosScale(allyRanged.position, displayScale);
        moved.Add(results.actor);
        //add self abilities?
        foreach(AbilityResult r in results.targets)
        {
            Debug.Log(r.target.Location);
            if (r.target.UnitTeam.isAlly)
            {
                r.target.SetPosScale(allyTargets[r.target.Location].position, displayScale);
            }
            else
            {
                r.target.SetPosScale(enemyTargets[r.target.Location].position, displayScale);
            }
            moved.Add(r.target);
            r.Display();
            //TODO check for death icons, etc.
        }
    }
    public void HideAciton()
    {
        gameObject.SetActive(false);
        foreach(Unit u in moved)
        {
            u.Moving = true;
            Debug.Log(u.TargetPosition);
        }
        Reset();
    }

    public void Reset()
    {
        moved.Clear();
    }
}
