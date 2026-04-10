using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents + " at: " + name); }}

    [Space(10)]

    [SerializeField] private soDATA_Item itemToBePickedUp;
    
    [SerializeField] private GameObject itemModel;
    [SerializeField] private Transform itemModelHandle;

    [SerializeField] private GameObject pickupPrompter;

    [SerializeField] private UnityEvent unityEventOnItemPickup;

    [SerializeField] private soDATA_ItemPickupRangeBalanceData pickupRangeBalanceData;
    private float pickupRange => pickupRangeBalanceData.STAT_pickupRange;
    [SerializeField] private AudioClip pickupSFX;

    public void Awake(){ Setup(); }
    [Button] public void Setup()
    {
            if (itemToBePickedUp == null){ Destroy(gameObject); return; }
            if (itemModel != null){ UpdateUX(false); return; }
        itemModel = Instantiate(itemToBePickedUp.STAT_itemModel, itemModelHandle);

        UpdateUX(false);
    }

    public void OnMouseOver()
    {
            Vector3 playerPos = FindAnyObjectByType<TAG_PLAYER>().transform.position;
            if (Vector3.Distance(playerPos, transform.position) > pickupRange){ UpdateUX(false); return; }
        UpdateUX(true);
        if (Input.GetKeyDown(KeyCode.F)){ Pickup(); return; }
    }
    public void OnMouseExit()
    {
        UpdateUX(false);
    }

    public void UpdateUX(bool isMouseOver = false)
    {
        pickupPrompter.SetActive(isMouseOver);
    }

    public void Pickup()
    {
        #region TEMP FIX ADD COLLECTABLE ITEM CLASS
        if (itemToBePickedUp.STAT_collectible){ GameManager.AddCollectable(ENUM_CollectableTypes.Flower); }
        #endregion
        else { Inventory.AddItem(itemToBePickedUp); }
        SFX.Play(pickupSFX, transform.position);
        unityEventOnItemPickup.Invoke();
        Destroy(gameObject); 
    }
}