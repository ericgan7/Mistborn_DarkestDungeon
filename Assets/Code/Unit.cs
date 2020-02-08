using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameState gs;
    [SerializeField] ProgressBar defenseBar;
    [SerializeField] ProgressBar healthbar;
    [SerializeField] ProgressBar willbar;
    [SerializeField] GameObject popupText;
    [SerializeField] Character character;
    [SerializeField] SelectionIcon selectionIcon;

    RectTransform rt;
    public Team UnitTeam { get; set; }
    public bool Active { get { return gameObject.activeSelf; } }
    public TargetedState targetedState;
    public int Location { get; set; }
    public Vector3 TargetPosition { get; set; }
    public bool Moving { get; set; }
    Vector3 velocity = Vector3.zero;
    Vector3 scalev = Vector3.zero;
    float elapsed = 0f;
    public float moveTime;

    public List<StatusIcon> statusIcons;
    public Stats stats;
    public StatusEffect test;
    public void UpdateUI()
    {
        healthbar.UpdateBar(character.health);
        willbar.UpdateBar(character.will);
        defenseBar.UpdateBar(character.defense);
        //TODO update status icon
    }

    void Awake()
    {
        gs = FindObjectOfType<GameState>();
        rt = GetComponent<RectTransform>();
        stats = new Stats(character, this);
        if (test != null) { stats.modifiers.Add(test); }
        TargetPosition = rt.localPosition;
    }

    void Start()
    {
        UpdateUI();
    }

    public void MoveUnit(Vector3 destination)
    {
        velocity = Vector3.zero;
        TargetPosition = destination;
        Moving = true;
        elapsed = 0f;
    }

    public void SetPosScale(Vector3 pos, Vector3 scale)
    {
        rt.position = pos;
        rt.localScale = scale;
        velocity = Vector3.zero;
        scalev = Vector3.zero;
        elapsed = 0f;
    }

    public void SetCharacter(Character c)
    {
        character = c;
        stats = new Stats(c, this);
    }

    public void Update()
    {
        if (Moving)
        {
            Vector3 newPos = Vector3.SmoothDamp(rt.localPosition, TargetPosition, ref velocity, moveTime);
            rt.localScale = Vector3.SmoothDamp(rt.localScale, Vector3.one, ref scalev, moveTime);
            elapsed += Time.deltaTime;
            if (elapsed > moveTime * 10)
            {
                Moving = false;
            }
            rt.localPosition = newPos;
        }
    }

    public void SetTargetState(TargetedState t)
    {
        targetedState = t;
        selectionIcon.SetTarget(targetedState);
    }

    public void SpawnText(string message, Color c)
    {
        GameObject text = Instantiate(popupText, gameObject.transform);
        PopupText pop = text.GetComponent<PopupText>();
        pop.SetMessage(message);
        pop.SetColor(c);
    }

    public void OnPointerClick(PointerEventData p)
    {
        gs.ic.SelectCharacter(this);
        
    }

    public void OnPointerEnter(PointerEventData p)
    {
        //highlight ui;
    }
    public void OnPointerExit(PointerEventData p)
    {
        //unhighlight ui;
    }
    
}
