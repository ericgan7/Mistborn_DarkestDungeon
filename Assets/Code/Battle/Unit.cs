using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameState gs;
    [SerializeField] GameObject popupText;
    [SerializeField] Character character;
    [SerializeField] Image sprite;
    [SerializeField] SelectionIcon selectionIcon;
    [SerializeField] StatusEffectManager statusEffects;
    [SerializeField] EffectUI tooltip;
    [SerializeField] HealthManager health;

    RectTransform rt;
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
    Vector3 velocity = Vector3.zero;
    Vector3 scalev = Vector3.zero;
    float elapsed = 0f;
    public float moveTime;
    float _time;

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
        health.ExecuteUpdate();
        tooltip.SetAbility(this, null, null);
    }

    public void SetUIChange(Vector2Int damageRange){
        health.SetDamageIndicators(damageRange);
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
        TargetPosition += destination;
        _time = time;
    }

    public void ResetPosition(){
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
    }

    public void SetPosScale(Vector3 pos, Vector3 scale)
    {
        rt.localPosition = pos;
        rt.localScale = scale;
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
        SetSprite(c.className + "_Default");
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

    public void CreatePopUpText(string message, Color color, float delay = 0f)
    {
        StartCoroutine(SpawnText(message, color, delay));
    }

    IEnumerator SpawnText(string message, Color color, float delay){
        yield return new WaitForSeconds(delay);
        GameObject text = Instantiate(popupText, gameObject.transform);
        PopupText pop = text.GetComponent<PopupText>();
        pop.SetMessage(message);
        pop.SetColor(color);
        pop.SetOffest(new Vector3(Random.Range(-50f, 50f), 250f, 0f));
    }

    public void SetSprite(string name, float time = 0.0f){
        if (time == 0.0f){
            Sprite s = SpriteLibrary.GetCharSprite(name);
            if (s == null){
                Debug.Log("Sprite is null. Missing " + name);
            }
            sprite.sprite = s;
        } else {
            StartCoroutine(DelayedSprite(name, time));
        }
    }

    IEnumerator DelayedSprite(string name, float time){
        yield return new WaitForSeconds(time);
        sprite.sprite = SpriteLibrary.GetCharSprite(name);
    }

    public void Die(){
        UnitTeam.MoveUnit(this, 4);
        SetSprite(character.className+"_Hurt");
        fade = true;
        TurnOffUIElemenets();
    }

    public void OnPointerClick(PointerEventData p)
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

    public void OnPointerEnter(PointerEventData p)
    {
        if (tooltip.currentAbility != null && showUI){
            tooltip.gameObject.SetActive(true);
            health.ShowIndicators(true);
        }
    }
    public void OnPointerExit(PointerEventData p)
    {
        tooltip.gameObject.SetActive(false);
        health.ShowIndicators(false);
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
}
