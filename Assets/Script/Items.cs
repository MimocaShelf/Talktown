using UnityEngine;

public enum ItemType
{
    None = 0,
    Apples, 
    Bread,
    Cereal,
    Milk,
    Water, 
    Chicken

}

public class Items : MonoBehaviour
{
    [Header("Item settings")]
    public ItemType itemType = ItemType.None;
    public string displayName = "Unknown Item";

    private bool playerInRange;
    private PlayerInventory playerInventory;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<PlayerInventory>();

            InteractionPrompt.Instance?.Show($"Press E to pick up {displayName}");
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;

            InteractionPrompt.Instance?.Hide();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerInRange || playerInventory == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory.TryPickup(itemType))
            {
                Debug.Log($"Picked up {displayName}");
                InteractionPrompt.Instance?.Hide();

            }InteractionPrompt.Instance?.Show($"Already holding {playerInventory.HeldItem}");
        }
    }
}
