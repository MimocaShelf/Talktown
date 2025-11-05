using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public GameObject currentWord;
    private TextMeshProUGUI slotText;   
    private Image background;

    private void Awake()
    {
        // Find the text component in this slot
        background = GetComponent<Image>();
        slotText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DraggableWord draggable = eventData.pointerDrag.GetComponent<DraggableWord>();
            TextMeshProUGUI wordText = eventData.pointerDrag.GetComponentInChildren<TextMeshProUGUI>();

            if (draggable != null && wordText != null)
            {
                // If a different word was here before, reset its color
                if (currentWord != null && currentWord != eventData.pointerDrag)
                {
                    Image oldImage = currentWord.GetComponent<Image>();
                    if (oldImage != null) oldImage.color = Color.white;
                }

                // Assign new word
                slotText.text = wordText.text;
                currentWord = eventData.pointerDrag;

                // Make visuals blue
                Image wordImage = currentWord.GetComponent<Image>();
                if (wordImage != null) wordImage.color = Color.blue;
                if (background != null) background.color = Color.blue;

            }
        }
    }
}
