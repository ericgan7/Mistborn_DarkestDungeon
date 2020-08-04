using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasicTooltip : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] TextMeshProUGUI text;

    public void SetWeapon(Weapon weapon){
        text.text = "";
        if (weapon ==null){
            return;
        }
        text.text += weapon.weaponName + "\n";
        text.text += string.Format("<color={0}><b>Damage:</b></color> {1}-{2}",
            ColorPallete.GetStatHexColor(StatType.damage), weapon.damage.x, weapon.damage.y);
        //accuracy, crit?
    }

    public void SetDescription(string description){
        text.text = description;
    }

    public bool HasText(){
        return text.text.Length > 0;
    }
}
