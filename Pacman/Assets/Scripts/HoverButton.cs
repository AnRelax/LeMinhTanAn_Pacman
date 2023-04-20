using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    private Color originalColor;

    void Start(){
        buttonImage = GetComponent<Image>();
        originalColor = buttonImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData){
        Color highlightColor = originalColor;
        highlightColor.a = 1f;
        buttonImage.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData){
        buttonImage.color = originalColor;
    }
}

