using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Mission/EnemyGroup")]
public class EnemyGroup : ScriptableObject
{
    public List<Character> characters;
    public int threat;
}
