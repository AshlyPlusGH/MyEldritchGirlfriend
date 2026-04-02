using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents + " at: " + name); }}

    [Space(10)]

    [SerializeField] private RectTransform selectedSlotPointerTransform;
    [SerializeField] private GenericDictionary<int,Image> itemUISlots = new();

    void Awake(){ Setup(); }
    public void Setup(){  }

    public void UpdateUI()
    {
        Dictionary<int,soDATA_Item> newItems = new(Inventory.STAT_contents);

        foreach (var itemSlot in itemUISlots)
        {
            if (!newItems.ContainsKey(itemSlot.Key))
            {
                itemSlot.Value.color = new Color(0,0,0,0);
                itemSlot.Value.sprite = null;
                
                continue;
            }

            itemSlot.Value.color = Color.white;
            itemSlot.Value.sprite = newItems[itemSlot.Key].STAT_sprite;
        }

        if ((itemUISlots.Count - 1) < Inventory.selectedSlot){ selectedSlotPointerTransform.gameObject.SetActive(false); return; }
        selectedSlotPointerTransform.gameObject.SetActive(true);
        selectedSlotPointerTransform.anchoredPosition = itemUISlots[Inventory.selectedSlot].transform.parent.GetComponent<RectTransform>().anchoredPosition;
    }
}