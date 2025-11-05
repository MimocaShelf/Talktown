using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Image heldItemIcon;

    [Header("Item Icons")]
    [SerializeField] private Sprite appleSprite;
    [SerializeField] private Sprite breadSprite;
    [SerializeField] private Sprite cerealSprite;
    [SerializeField] private Sprite milkSprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite chipsSprite;
    [SerializeField] private Sprite bandaidSprite;

    private void Awake() => Instance = this;

    public void Refresh(ItemType held)
    {
        //if (itemLabel) itemLabel.text = held == ItemType.None ? "Empty" : held.ToString();
        if (heldItemIcon) heldItemIcon.enabled = (held != ItemType.None);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (playerInventory == null) return;

        if (!playerInventory.HasItem)
        {
            heldItemIcon.sprite = null;
            heldItemIcon.enabled = false;
            return;
        }

        heldItemIcon.enabled = true;

        switch (playerInventory.HeldItem)
        {
            case ItemType.Apples:
                heldItemIcon.sprite = appleSprite;
                break;
            case ItemType.Milk:
                heldItemIcon.sprite = milkSprite;
                break;
            case ItemType.Chips:
                heldItemIcon.sprite = chipsSprite;
                break;
            case ItemType.Water:
                heldItemIcon.sprite = waterSprite;
                break;
            case ItemType.Bread:
                heldItemIcon.sprite = breadSprite;
                break;
            //case ItemType.Bandaids:
                //heldItemIcon.sprite = bandaidSprite;
                //break;
            default:
                heldItemIcon.sprite = null;
                break;
        }
    }
}