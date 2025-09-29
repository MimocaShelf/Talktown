using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Slot : MonoBehaviour, IDropHandler
{
    public GameObject currentWord;
    private TextMeshProUGUI slotText;

    private void Awake()
    {
        // Find the text component in this slot
        slotText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            RectTransform word = eventData.pointerDrag.GetComponent<RectTransform>();

            // Get the text from the dragged word
            TextMeshProUGUI wordText = eventData.pointerDrag.GetComponentInChildren<TextMeshProUGUI>();

            if (wordText != null && slotText != null)
            {
                slotText.text = wordText.text;  // ✅ Display the word on the slot
            }

            // Track which word is here
            currentWord = eventData.pointerDrag;

            Debug.Log($"[SLOT] {gameObject.name} now contains word: {slotText.text}");
        }
    }
}
