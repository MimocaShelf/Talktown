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
            DialogueManager.Instance.ShowDialogue(npcName + ": " + dialogueLine);
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
