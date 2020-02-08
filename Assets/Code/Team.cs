using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Team : MonoBehaviour
{
    [SerializeField] List<Unit> units;
    public bool isAlly;
    public List<Vector3> positions;
    int direction;

    private void Awake()
    {
        direction = isAlly ? -1 : 1;
        positions = new List<Vector3>(units.Count);
        for (int i = 0; i < units.Count; ++i)
        {
            units[i].Location = i;
            units[i].UnitTeam = this;
            positions.Add(units[i].transform.localPosition);
        }
    }

    public List<Unit> GetUnits()
    {
        List<Unit> result = new List<Unit>();
        foreach(Unit u in units){
            if (u.Active)
            {
                result.Add(u);
            }
        }
        return result;
    }

    public void MoveUnit(Unit target, int amount)
    {
        int index = target.Location;
        int newIndex = Mathf.Clamp(index + amount, 0, GetUnits().Count);
        units.Remove(target);
        units.Insert(newIndex, target);
        for (int i = 0; i < units.Count; ++i)
        {
            units[i].Location = i;
            units[i].MoveUnit(positions[i]);
        }
    }
}
