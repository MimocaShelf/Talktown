using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            InventoryManager.Instance.CollectItem(itemName);
            DialogueManager.Instance.ShowDialogue("Picked up: " + itemName);
            Destroy(gameObject);
        }
    }
}
