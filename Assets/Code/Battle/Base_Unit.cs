using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.LWRP;

public class Base_Unit : MonoBehaviour//, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //serialize fields
    [SerializeField] Image sprite;                  //Character Sprite
    [SerializeField] Image effectSprite;            //effect overlay
    [SerializeField] Image backgroundSprite;        //background of unit
    [SerializeField] Image deathblowSprite;         //unused currently
    [SerializeField] ParticleSystem particleEffects;//particle effects
    [SerializeField] Light2D hurtLight;
    [SerializeField] Light2D attackLight;

    //positional
    [SerializeField] RectTransform rectTransform;
        //location

    
    //Function Components
    [SerializeField] Team unitTeam;
    [SerializeField] SelectionIcon selection;
    [SerializeField] HealthManager healthManager;

    Stats stats;                                    //unit stats. Should be created in Team, as it can be shared between units
    [SerializeField] StatusEffectManager statusEffects; //needs to be refactored
    [SerializeField] EnemyAI aI;                    //if unit is enemy, it should have ai
        //effect ui
    

    //Smooth Damp Moving Variables
    public Vector3 TargetPosition { get; set; }
    public Vector3 targetScale;
    public bool Moving { get; set; }
    bool fade;
    float fadevelocity;
    protected Vector3 velocity = Vector3.zero;
    protected Vector3 scalev = Vector3.zero;
    protected float elapsed = 0f;
    public float moveTime;
    protected float _time;

}