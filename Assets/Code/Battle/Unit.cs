using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.LWRP;

public class Unit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected GameState gs;
    [SerializeField] SpawnText popups;
    [SerializeField] Character character;
    [SerializeField] Image sprite;
    [SerializeField] Image effectSprite;
    [SerializeField] Image deathblowSprite;
    [SerializeField] Image backSprite;
    [SerializeField] SelectionIcon selectionIcon;
    [SerializeField] StatusEffectManager statusEffects;
    [SerializeField] EffectUI tooltip;
    [SerializeField] HealthManager health;
    [SerializeField] Light2D hurtLight;
    [SerializeField] Light2D attackLight;
    [SerializeField] ParticleSystem particleEffects;

    protected RectTransform rt;
    public Team UnitTeam { get; set; }
    public bool Active { get { return gameObject.activeSelf; } }
    public TargetedState targetedState;
    public int Location { get; set; }

    //movement 
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

    public Stats stats;
    public EnemyAI ai;

    //ui
    public bool showUI;

    void Awake()
    {
        gs = FindObjectOfType<GameState>();
        rt = GetComponent<RectTransform>();
        TargetPosition = rt.localPosition;
        _time = moveTime;
        Moving = false;
        showUI = true;
        gameObject.SetActive(false);
        //TODO replace fade out with death animation.
        fade = false;
        fadevelocity = 0f;
    }

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (character == null){
            return;
        }
        if (health != null){
            health.ExecuteUpdate();
        }
        tooltip.SetAbility(this, null, null);
    }

    public void SetHealthChange(Vector2Int range){
        if (range.y > 0){
            health.SetHealIndicators(range);
        }
        else{
            health.SetDamageIndicators(range);
        }
    }

    public void SetStressChange(int range){
        health.SetStressIndicators(range);
    }


    public void MoveUnit(Vector3 destination, Vector3 scale, float time = -1f)
    {
        //position
        velocity = Vector3.zero;
        TargetPosition = destination;
        //scale
        scalev = Vector3.zero;
        targetScale = scale;
        //flags and elapsed time
        Moving = true;
        elapsed = 0f;
        if (time >= 0f){
            _time = time;
        } else {
            _time = moveTime;
        }
    }

    public void UpdateUnitPosition(Vector3 destination, float time = 1.0f){
        velocity = Vector3.zero;
        scalev = Vector3.zero;
        Moving = true;
        elapsed = 0f;
        _time = time;
        TargetPosition += destination;
    }

    public virtual void ResetPosition(){
        //position
        velocity = Vector3.zero;
        TargetPosition = UnitTeam.positions[Location];
        //scale
        scalev = Vector3.zero;
        targetScale = Vector3.one;
        //flags and elapsed time
        Moving = true;
        elapsed = 0f;
        _time = moveTime;
        //set action lights to disabled
        attackLight.gameObject.SetActive(false);
        hurtLight.gameObject.SetActive(false);
    }

    public void SetPosScale(Vector3 pos, Vector3 scale)
    {
        rt.localPosition = pos;
        rt.localScale = scale;
        targetScale = scale;
        TargetPosition = pos;
        velocity = Vector3.zero;
        scalev = Vector3.zero;
        elapsed = 0f;
    }

    public void SetCharacter(Character c, bool isAlly)
    {
        character = c;
        stats = new Stats(c, this, statusEffects);
        statusEffects.SetCharacter(c);
        stats.Init();
        ai = c.ai;
        SetSprite(c.GetSpriteHeader() + "_Default");
        if (!isAlly)
        {
            sprite.rectTransform.localScale = new Vector3(-1, 1, 1);
        }
        health.InitBars();
        UpdateUI();
    }

    public void Update()
    {
        if (Moving)
        {
            rt.localPosition = Vector3.SmoothDamp(rt.localPosition, TargetPosition, ref velocity, _time);
            rt.localScale = Vector3.SmoothDamp(rt.localScale, targetScale, ref scalev, _time);
            elapsed += Time.deltaTime;
            if (elapsed > moveTime * 10)
            {
                Moving = false;
            }
        }
        if (fade){
            sprite.color = new Color( 1, 1, 1, Mathf.SmoothDamp(sprite.color.a, 0, ref fadevelocity, 1.5f));
            if (sprite.color.a < 0.01){
                gameObject.SetActive(false);
                fade = false;
            }
        }
    }

    public void SetTargetState(TargetedState t)
    {
        targetedState = t;
        selectionIcon.SetTarget(targetedState);
    }

    public void SetToolTip(Unit actor, Ability ability)
    {
        tooltip.SetAbility(actor, ability, this);
    }

    public void SetToolTipActive(bool isActive){
        if (targetedState == TargetedState.Targeted){
            tooltip.gameObject.SetActive(isActive);
        }
    }

    public void CreatePopUpText(string message, Color color)
    {
        popups.QueueMessage(message, color);
    }

    public bool IsFinishedPopupText(){
        return popups.IsEmpty();
    }

    public enum UnitLight{ attack, hurt }
    public void SetLight(UnitLight light, bool isOn){
        switch(light){
            case UnitLight.attack:
                attackLight.gameObject.SetActive(isOn);
                break;
            case UnitLight.hurt:
                hurtLight.gameObject.SetActive(isOn);
                break;
        }
    }

    public void SetSprite(string name, float time = 0.0f){
        if (time == 0.0f){
            Sprite s = SpriteLibrary.GetCharSprite(name);
            if (s == null){
                Debug.Log("Sprite is null. Missing " + name);
            }
            sprite.sprite = s;
        } else if (gameObject.activeInHierarchy){
            StartCoroutine(DelayedSprite(name, time));
        }
    }

    IEnumerator DelayedSprite(string name, float time){
        yield return new WaitForSeconds(time);
        sprite.sprite = SpriteLibrary.GetCharSprite(name);
    }

    public void SetEffectSprite(string name){
        if (name == null){
            effectSprite.sprite = null;
            effectSprite.enabled = false;
        }
        else {
            effectSprite.sprite = SpriteLibrary.GetAbilityEffectSprite(name);
            effectSprite.enabled = true;
        }
    }

    public void SetParticleEffect(AbilityParticleEffect abilityParticle){
        
        if (abilityParticle == null){
            backSprite.enabled = false;
        }
        else {
            backSprite.enabled = true;
            backSprite.color = abilityParticle.color;
            particleEffects.textureSheetAnimation.SetSprite(0, abilityParticle.particle);
            var velocity = particleEffects.velocityOverLifetime;
            velocity.y = abilityParticle.direction;
            var color = particleEffects.main;
            color.startColor = abilityParticle.color;
            particleEffects.Play();
        }
    }

    public void Die(){
        UnitTeam.MoveUnit(this, 4);
        SetSprite(character.GetSpriteHeader() +"_Hurt");
        fade = true;
        TurnOffUIElemenets();
    }

    public virtual void OnPointerClick(PointerEventData p)
    {
        if (p.button == PointerEventData.InputButton.Left)
        {
            gs.ic.SelectCharacter(this);
        }
        else if (p.button == PointerEventData.InputButton.Right){
            if (!gs.ic.GetBlocked() && UnitTeam.isAlly){
                gs.uic.SetCharacterMenu(this, character);
            }
        }
    }

    public virtual void OnPointerEnter(PointerEventData p)
    {
        if (tooltip.currentAbility != null && showUI){
            if (tooltip.currentAbility.isAOE){
                foreach(Unit u in UnitTeam.GetUnits()){
                    u.SetChangeIndicators(true);
                    u.SetToolTipActive(true);
                }
            }
            else {
                SetChangeIndicators(true);
                tooltip.gameObject.SetActive(true);
            }
        }
    }
    public virtual void OnPointerExit(PointerEventData p)
    {
        if (tooltip.currentAbility == null){
            return;
        }
        if (tooltip.currentAbility.isAOE){
            foreach(Unit u in UnitTeam.GetUnits()){
                u.SetChangeIndicators(false);
                u.SetToolTipActive(false);
            }
        }
        else {
            SetChangeIndicators(false);
            tooltip.gameObject.SetActive(false);
        }
    }
    
    public void TurnOffUIElemenets(){
        showUI = false;
        health.gameObject.SetActive(false);
        tooltip.gameObject.SetActive(false);
        statusEffects.ShowUI(false);
    }

    public void TurnOnUIElements(){
        showUI = true;
        health.gameObject.SetActive(true);
        statusEffects.ShowUI(true);
    }

    public void SetActionPips(bool isOn){
        if (isOn){
            health.AddActionIndicator();
        }
        else {
            health.RemoveActionIndicator();
        }
    }

    public void SetChangeIndicators(bool isOn){
        if (targetedState == TargetedState.Targeted){
            health.ShowIndicators(isOn);
        }
        
    }

    public void Panic(){
        gs.gc.StartCoroutine(gs.gc.ExecuteInitialAfflictionTurn(this));
        health.SetAffliction(true);
    }
}
