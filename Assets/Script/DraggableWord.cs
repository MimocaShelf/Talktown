using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class DraggableWord : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform parentAfterDrag;
    private Canvas canvas;
    private Transform originalParent;
    private TextMeshProUGUI wordText;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(canvas.transform); // bring to top
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public string GetWord()
    {
        return wordText != null ? wordText.text : "";
    }

    public void SetParent(Transform newParent)
    {
        parentAfterDrag = newParent;
    }
}
