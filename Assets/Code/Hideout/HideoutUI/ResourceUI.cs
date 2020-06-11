using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    public TextMeshProUGUI gold;
    public TextMeshProUGUI intel;
    public TextMeshProUGUI metal;
    public TextMeshProUGUI weapons;

    GameState state;
    ResourceManager resources;

    public void Awake(){
        state = FindObjectOfType<GameState>();
        resources = state.resources;
    }

    public void UpdateUI(){
        gold.text = resources.gold.ToString();
        intel.text = resources.intel.ToString();
        metal.text = resources.metal.ToString();
        weapons.text = resources.weaponParts.ToString();
    }
}
