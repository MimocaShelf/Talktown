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
            RectTransform word = eventData.pointerDrag.GetComponent<RectTransform>();

            // Get the text from the dragged word
            TextMeshProUGUI wordText = eventData.pointerDrag.GetComponentInChildren<TextMeshProUGUI>();

            DraggableWord draggable = eventData.pointerDrag.GetComponent<DraggableWord>();
            if (draggable != null)
            {
                // Update the slot’s text
                slotText.text = draggable.GetWord();

                // Change slot background colour to blue
                if (background != null)
                    background.color = Color.blue;

                Debug.Log($"Slot filled with: {slotText.text}");
            }


            if (wordText != null && slotText != null)
            {
                slotText.text = wordText.text;  // ✅ Display the word on the slot
            }
            Image wordImage = eventData.pointerDrag.GetComponent<Image>();
            if (wordImage != null)
            {
                wordImage.color = Color.blue;
            }
            Image slotImage = GetComponent<Image>();
            if (slotImage != null)
            {
                slotImage.color = Color.blue;
            }

            // Track which word is here
            currentWord = eventData.pointerDrag;

            Debug.Log($"[SLOT] {gameObject.name} now contains word: {slotText.text}");
        }
    }
}
