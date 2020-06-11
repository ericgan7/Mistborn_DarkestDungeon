using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemObj : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public TextMeshProUGUI count;
    public ItemInstance item;

    public ItemSlot slot;
    GameState state;
    GraphicRaycaster raycaster;
    bool isDragging;
    public RectTransform rt;
    [SerializeField] TextMeshProUGUI tooltip;
    [SerializeField] RectTransform background;

    public void Awake()
    {
        icon = GetComponent<Image>();
        count = GetComponentInChildren<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();
        slot = GetComponentInParent<ItemSlot>();
        slot.currentItem = this;
        state = FindObjectOfType<GameState>();
        background.gameObject.SetActive(false);
    }

    void Start(){
        raycaster = FindObjectOfType<GameState>().uic.uiRayCaster;
    }

    public void UpdateItem(ItemInstance itemInstance)
    {
        if (itemInstance.itemType){
            item = itemInstance;
            icon.sprite = item.itemType.icon;
            gameObject.SetActive(true);
        }
        else {
            gameObject.SetActive(false);
        }
        if (item == null){
            count.text = "";
        }else {
            count.text = item.amount.ToString();
        }
        SetTooltip();
    }

    public void SetTooltip(){
        background.gameObject.SetActive(true);
        if (item == null){
            tooltip.text = "";
        }
        else {
            tooltip.text = item.itemType.ToString();
        }
        background.sizeDelta = new Vector2(LayoutUtility.GetPreferredWidth(tooltip.rectTransform),
                                            LayoutUtility.GetPreferredHeight(tooltip.rectTransform));
        background.gameObject.SetActive(false);
    }

    void Update(){
        if (isDragging){
            rt.position = Input.mousePosition;
        }
    }
    public void OnPointerDown(PointerEventData p)
    {
        if (p.button == PointerEventData.InputButton.Left){
            isDragging = true;
            Debug.Log("DRAG ITEM");
        } else if(p.button == PointerEventData.InputButton.Right &&
        state.gc.currentUnit.UnitTeam.isAlly){
            int amount = item.UseItem(state.gc.currentUnit, state);
            if (amount == 0){
                slot.currentItem = null;
                Destroy(gameObject);
            } else {
                count.text = amount.ToString();
            }
        }
    }

    public void OnPointerUp(PointerEventData p){
        isDragging = false;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(p, results);
        foreach (RaycastResult r in results){
            ItemSlot s = r.gameObject.GetComponent<ItemSlot>();
            if (s != null && s != slot){
                slot.SwapItems(s);
                return;
            }
        }
        slot.UpdateItemPosition();
    } 

    public void OnPointerEnter(PointerEventData pointer){
        background.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointer){
        background.gameObject.SetActive(false);
    }

    public void SetIconUsable(Unit currentUnit, bool isCombat){
        if (item.itemType.IsUsable(currentUnit, isCombat)){
            icon.color = Color.white;
        } else {
            icon.color = new Color(1f,1f,1f, 0.5f);
        }
    }


}
