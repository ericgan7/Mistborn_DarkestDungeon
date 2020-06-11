using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Class")]
public class CharacterClass : ScriptableObject
{
    public string className;
    public Sprite classIcon;

    public List<Ability> classAbilities;
    public List<TraitTarget> attackBonuses;
    //TODO: implement?
    public List<StatusEffect> traitEffects;
}
