using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public string npcName = "NPC";
    [TextArea] public string dialogueLine = "Hello, welcome to Talktown!";
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager.Instance.ShowDialogue(npcName + ": Welcome to Talktown!");

            DialogueChoiceManager.Instance.ShowChoices(
                "Ask about the town", () => DialogueManager.Instance.ShowDialogue("This town is full of life!"),
                "Ask about work", () => DialogueManager.Instance.ShowDialogue("There are many jobs in the market."),
                "Ask about food", () => DialogueManager.Instance.ShowDialogue("Try the local café!"),
                "Say goodbye", () => DialogueManager.Instance.ShowDialogue("Goodbye! Come back anytime.")
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            DialogueManager.Instance.ShowDialogue("Press E to talk to " + npcName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            DialogueManager.Instance.ClearDialogue();
        }
    }
}
