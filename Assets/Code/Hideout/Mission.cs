using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionDistrict{
    Cracks,
}

public enum MissionType {
    Robbery,
}
[CreateAssetMenu(menuName="Mission/Mission")]
public class Mission : ScriptableObject
{
    public MissionDistrict district;
    public MissionType type;
    public int difficulty;
    //public List<ItemReward> rewards;
    public Vector2 position;
    public int maxTeamSize;
}

public static class MissionString{
    public static string MissionTypeString(MissionType m){
        switch (m){
            case MissionType.Robbery:
                return "Smash and Grab";
        }
        return "";
    }
    public static string DistrictString(MissionDistrict m){
        switch(m){
            case MissionDistrict.Cracks:
                return "The Cracks";
        }
        return "";
    }
}