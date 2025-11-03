using UnityEngine;

public enum ItemType
{
    None,
    Apples,
    Bread,
    Cereal,
    Milk,
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
                Debug.Log($"✅ Player picked up: {displayName}");
                InteractionPrompt.Instance?.Hide();
            }
            else
            {
                Debug.Log($"❌ Can't pick up {displayName}, already holding {_playerInv.HeldItem}");
                InteractionPrompt.Instance?.Show($"Hands full (holding {_playerInv.HeldItem}).");
            }
        }
    }
}
