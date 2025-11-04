using UnityEngine;

public enum ItemType
{
    None,
    Apples,
    Milk,
    Chips,
    Water,
    Chicken
}

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemType itemType = ItemType.Apples;
    [SerializeField] private string displayName = "Apples";

    private bool _playerInRange;
    private PlayerInventory _playerInv;

    public string itemName;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _playerInRange = true;
        _playerInv = other.GetComponent<PlayerInventory>();
        InteractionPrompt.Instance?.Show($"Press E to pick up {displayName}");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _playerInRange = false;
        _playerInv = null;
        InteractionPrompt.Instance?.Hide();
    }

    private void Update()
    {
        if (!_playerInRange || _playerInv == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_playerInv.TryPickup(itemType))
            {
                InteractionPrompt.Instance?.Hide();
            }
            else
            {
                InteractionPrompt.Instance?.Show($"Hands full (holding {_playerInv.HeldItem}).");
            }
        }
    }
}
