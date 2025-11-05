using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public TextMeshProUGUI dialogueText;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ShowDialogue(string message)
    {
        dialogueText.text = message;
    }

    public void ClearDialogue()
    {
        dialogueText.text = "";
    }
    public bool IsShowing()
    {
        return dialogueText != null
               && !string.IsNullOrEmpty(dialogueText.text)
               && dialogueText.gameObject.activeSelf;
    }
}
