using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class ItemSlotPhysical : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents + " at: " + name); }}

    [Space(10)]

    [SerializeField] private List<soDATA_Item> allowedItems = new();
    [SerializeField] private Transform placedItemHandle;

    [SerializeField] private UnityEvent unityEventOnItemPlaced;
    [SerializeField] private UnityEvent unityEventOnItemRemoved;

    [SerializeField] private GameObject prompter;

    private GameObject placedItemObject;
    private soDATA_Item placedItem = null;

    void Awake(){ Setup(); }
    public void Setup(){ prompter.SetActive(false); }

    public void OnMouseOver()
    {
        prompter.SetActive(true);
            if (!Input.GetMouseButtonDown(0)){ return; }
        if (placedItem == null){ InputPlaceItem(); }
        else { InputTakePlacedItem(); }
    }
    public void OnMouseExit(){ prompter.SetActive(false); }

        void InputPlaceItem()
        {
                if (placedItem != null){ return; }
            soDATA_Item selectedItem = Inventory.GetSelectedItem();
                if (selectedItem == null){ return; }
                if (!allowedItems.Contains(selectedItem)){ return; }
            Inventory.RemoveSelectedItem();
            PlaceItem(selectedItem); 
        }
        void InputTakePlacedItem()
        {
            TakePlacedItem(); 
        }

    public void PlaceItem(soDATA_Item item)
    {
            if (placedItem != null){ return; }
        placedItem = item;
        placedItemObject = Instantiate(item.STAT_itemModel,placedItemHandle);

        unityEventOnItemPlaced.Invoke();
    }

    [Button] public void TakePlacedItem()
    {
        Inventory.AddItem(placedItem);
        Destroy(placedItemObject);
        placedItem = null;

        unityEventOnItemRemoved.Invoke();
    }
}