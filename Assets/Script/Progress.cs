using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{

    public static Progress Instance;
    [Header("Ticks in order")]
    public Image groceryTick;
    public Image pharmacyTick;
    public Image friendTick;

    private void Awake()
    {
        Instance = this;

        if (groceryTick) groceryTick.gameObject.SetActive(false);
        if (pharmacyTick) pharmacyTick.gameObject.SetActive(false);
        if (friendTick) friendTick.gameObject.SetActive(false);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetGroceryDone()
    {
        if (groceryTick) groceryTick.gameObject.SetActive(true);
    }

    public void SetPharmacyDone()
    {
        if (pharmacyTick) pharmacyTick.gameObject.SetActive(true);
    }

    public void SetFriendDone()
    {
        if (friendTick) friendTick.gameObject.SetActive(true);
    }
}
