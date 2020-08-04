using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.LWRP;

public class UnitSelection : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Character currentCharacter;
    SelectionManager selectionManager;

    [SerializeField] RectTransform rt;
    [SerializeField] Image raycastTarget;
    [SerializeField] Image characterSprite;
    [SerializeField] Image highlight;
    [SerializeField] Light2D bottomLight;
    bool highlighted = false;

    Vector2 originPosition;
    Vector2 targetPosition;
    Vector2 velocity;
    bool moving = false;
    [SerializeField] float moveTime;

    float targetAlpha;
    float alphaVelocity;
    bool fading = false;
    [SerializeField] float alphaTime;

    float targetIntensity;
    float intensityVelocity;
    bool lightChanging;

    public void Init(Character character, SelectionManager manager){
        currentCharacter = character;
        selectionManager = manager;
        originPosition = rt.anchoredPosition;
        characterSprite.sprite = SpriteLibrary.GetCharSprite(character.GetSpriteHeader() + "_Default");
        characterSprite.SetNativeSize();
    }

    public void SetTeamHighlight(bool isTeam){
        highlighted = isTeam;
        highlight.gameObject.SetActive(isTeam);
    }

    public void Select(bool isSelected){
        if (isSelected){
            selectionManager.MoveLight(rt);
        }
    }

    public Vector2 Position(){
        return rt.anchoredPosition;
    }

    public Vector2 Origin(){
        return originPosition;
    }

    public void SetRaycast(bool isOn){
        raycastTarget.raycastTarget = isOn;
    }

    public void SetPositionImmediate(Vector2 position){
        rt.anchoredPosition = position;
    }

    public void SetPositionMoving(Vector2 position){
        targetPosition = position;
        velocity = Vector2.zero;
        moving = true;
    }

    public void SetAlphaMoving(float alpha){
        targetAlpha = alpha;
        alphaVelocity = 0f;
        fading = true;
    }

    public void SetLightMoving(float intensity){
        if (intensity == 0){
            highlight.gameObject.SetActive(false);
        } else {
            highlight.gameObject.SetActive(highlighted);
        }
        targetIntensity = intensity;
        intensityVelocity = 0f;
        lightChanging = true;
    }

    public void Update(){
        if (moving){
            rt.anchoredPosition = Vector2.SmoothDamp(rt.anchoredPosition, targetPosition, ref velocity, moveTime);
            if (Mathf.Abs(rt.anchoredPosition.x - targetPosition.x) + Mathf.Abs(rt.anchoredPosition.y - targetPosition.y) < 0.01){
                moving = false;
            }
        }
        if (fading){
            float alpha = Mathf.SmoothDamp(characterSprite.color.a, targetAlpha, ref alphaVelocity, alphaTime);
            characterSprite.color = new Color(1,1,1, alpha);
            //highlight.color = new Color(highlight.color.r, highlight.color.g, highlight.color.b, alpha);
            if (Mathf.Abs(characterSprite.color.a - targetAlpha) < 0.01){
                fading = false;
            }
        }
        if (lightChanging){
            bottomLight.intensity = Mathf.SmoothDamp(bottomLight.intensity, targetIntensity, ref intensityVelocity, alphaTime);
            if (Mathf.Abs(bottomLight.intensity - targetIntensity) < 0.01){
                lightChanging = false;
            }
        }
    }

    public void OnPointerClick(PointerEventData p){
        if (p.button == PointerEventData.InputButton.Left){
            selectionManager.SetCharacter(currentCharacter);
        }
        else if (p.button == PointerEventData.InputButton.Right){
            selectionManager.ChangeCharacterMenu();
        }
    }

    public void OnPointerEnter(PointerEventData p){
        selectionManager.HighlightCharacter(currentCharacter);
    }

    public void OnPointerExit(PointerEventData p){
        selectionManager.UnHighlightCharacter();
   }
}
