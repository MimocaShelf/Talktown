using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    [SerializeField] private TextMeshProUGUI itemLabel;
    [SerializeField] private Image itemIcon;

    private void Awake() => Instance = this;

    public void Refresh(ItemType held)
    {
        if (itemLabel) itemLabel.text = held == ItemType.None ? "Empty" : held.ToString();
        if (itemIcon) itemIcon.enabled = (held != ItemType.None);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
