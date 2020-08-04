using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gold;
    [SerializeField] TextMeshProUGUI intel;
    [SerializeField] TextMeshProUGUI metal;
    [SerializeField] TextMeshProUGUI supplies;
    [SerializeField] TextMeshProUGUI components;

    void Start(){
        UpdateUI();
    }

    public void UpdateUI(){
        gold.text = GameState.Instance.resources.gold.ToString();
        intel.text = GameState.Instance.resources.intel.ToString();
        metal.text = GameState.Instance.resources.metal.ToString();
        supplies.text = GameState.Instance.resources.supplies.ToString();
        components.text = GameState.Instance.resources.components.ToString();
    }

    public static bool HasResource(ResourceType type, int amount){
        switch(type){
            case ResourceType.gold:
                return GameState.Instance.resources.gold > amount;
            case ResourceType.intel:
                return GameState.Instance.resources.intel > amount;
            case ResourceType.metal:
                return GameState.Instance.resources.metal > amount;
            case ResourceType.supplies:
                return GameState.Instance.resources.supplies > amount;
            case ResourceType.components:
                return GameState.Instance.resources.components > amount;
            default:
                Debug.Log("Unknown Resources");
                return false;
        }
    }

    public static void SpendResource(ResourceType type, int amount){
        switch(type){
            case ResourceType.gold:
                GameState.Instance.resources.gold += amount;
                break;
            case ResourceType.intel:
                GameState.Instance.resources.intel += amount;
                break;
            case ResourceType.metal:
                GameState.Instance.resources.metal += amount;
                break;
            case ResourceType.supplies:
                GameState.Instance.resources.supplies += amount;
                break;
            case ResourceType.components:
                GameState.Instance.resources.components += amount;
                break;
            default:
                Debug.Log("Unknown Resource");
                break;
        }
    }
}
