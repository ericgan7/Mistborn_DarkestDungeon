using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterController : MonoBehaviour
{
    public GameState state;
    public HeroRosterSlot prefab;
    public GraphicRaycaster raycaster;
    public DetailedCharacterMenu characterMenu;
    public void Awake(){
        state = FindObjectOfType<GameState>();
        List<Character> characters = state.hr.currentRoster;
        foreach (Character c in characters){
            HeroRosterSlot hrs = Instantiate<HeroRosterSlot>(prefab, transform);
            //hrs.Init(c);
            //hrs.SetRaycaster(raycaster);
            //hrs.menu = characterMenu;
        }
    }
}
