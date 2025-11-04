using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{


    public ItemType HeldItem { get; private set; } = ItemType.None;

    public bool HasItem => HeldItem != ItemType.None;

    private float dropMessageTimer = 0f;

    public bool TryPickup(ItemType type)
    {
        if (HasItem) return false;

        HeldItem = type;
        InventoryUI.Instance?.Refresh(HeldItem);
        return true;
    }

    public bool TryConsume(ItemType type)
    {
        if (HeldItem != type) return false;

        HeldItem = ItemType.None;
        InventoryUI.Instance?.Refresh(HeldItem);
        return true;
    }

    public void DropItem()
    {
        HeldItem = ItemType.None;
        InventoryUI.Instance?.Refresh(HeldItem);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && HasItem)
        {
            DiscardItem();
        }

        if (dropMessageTimer > 0f)
        {
            dropMessageTimer -= Time.deltaTime;
            if (dropMessageTimer <= 0f)
            {
                InteractionPrompt.Instance?.Hide();
            }
        }
    }

    private void DiscardItem()  
    {
        HeldItem = ItemType.None;
        InventoryUI.Instance?.Refresh(HeldItem);
        DialogueManager.Instance?.ShowDialogue("You dropped the item.");
        dropMessageTimer = 1.5f;
    }
}
