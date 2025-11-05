using UnityEngine;

public enum ItemType
{
    None,
    // grocery
    Apples,
    Milk,
    Chips,
    Water,
    Bread,
    // pharmacy
    Syrup,
    EyeDrops,
    Lotion
}

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemType itemType = ItemType.Apples;
    [SerializeField] private string displayName = "Apples";

    private bool _playerInRange;
    private PlayerInventory _playerInv;
    private bool _justPickedUp;
    private float _pickupCooldown = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInRange = true;
        _playerInv = other.GetComponent<PlayerInventory>();

        if (_playerInv != null)
        {
            if (_playerInv.HasItem)
                DialogueManager.Instance.ShowDialogue("You’re already holding an item! Press Q to drop it first.");
            else
                DialogueManager.Instance.ShowDialogue($"Press E to pick up {displayName}.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInRange = false;
        _playerInv = null;
        _justPickedUp = false;

        DialogueManager.Instance.ClearDialogue();
    }

    private void Update()
    {
        if (!_playerInRange || _playerInv == null) return;

        if (_justPickedUp)
        {
            _pickupCooldown -= Time.deltaTime;
            if (_pickupCooldown <= 0f)
            {
                _justPickedUp = false;
                _pickupCooldown = 1.0f;
            }
            return;
        }

        if (_playerInv.HasItem)
        {
            DialogueManager.Instance.ShowDialogue("You’re already holding an item! Press Q to drop it first.");
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_playerInv.TryPickup(itemType))
            {
                DialogueManager.Instance.ShowDialogue($"Picked up {displayName}!");
                _justPickedUp = true;

                CancelInvoke(nameof(ClearDialogue));
                Invoke(nameof(ClearDialogue), 1.5f);
            }
        }
    }

    private void ClearDialogue()
    {
        DialogueManager.Instance.ClearDialogue();
    }
}
