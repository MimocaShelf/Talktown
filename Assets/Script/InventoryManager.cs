using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private HashSet<string> collectedItems = new HashSet<string>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CollectItem(string itemName)
    {
        collectedItems.Add(itemName);
    }

    public bool HasItem(string itemName)
    {
        return collectedItems.Contains(itemName);
    }
}