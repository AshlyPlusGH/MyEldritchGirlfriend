using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents + " at: " + name); }}

    [Space(10)]

    [SerializeField] private List<soDATA_Item> DEBUG_addContents = new();

    private static Dictionary<int,soDATA_Item> contents = new();

    public static Dictionary<int,soDATA_Item> STAT_contents => contents;

    public static int selectedSlot {get; private set;} = 0;

    static MinMaxInt inventorySize = new MinMaxInt(0,9);

    #region INSTANCECALLS
        public void Awake(){ Setup(); }
        public void Setup(){ UpdateUI(); }
        public void Update(){ GetInput(); }
        void GetInput()
        {
            ScrollInput();

            UseItemInput();
        }
            void ScrollInput()
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel");

                if (scroll == 0){ return; }
                if (scroll > 0){ SetSelectedSlot(selectedSlot + 1); }
                if (scroll < 0){ SetSelectedSlot(selectedSlot - 1); }

                UpdateUI();
            }
            void UseItemInput()
            {
                if (!Input.GetMouseButtonDown(0)){ return; }
                UseItem(selectedSlot);

                UpdateUI();
            }
    #endregion

    #region PUBLICSTATICCALLS
        public static void AddItem(soDATA_Item itemToAdd)
        {
            if (QueryIsFull()){ Debug.Log("Item was added but Inventory was too full!"); }

            if (GetSelectedItem() == null){ contents.Add(selectedSlot,itemToAdd); UpdateUI(); return; }
            for (int i = 0; i <= inventorySize.max; i++)
            {
                if (!contents.ContainsKey(i)){ contents.Add(i,itemToAdd); break; }
            }

            UpdateUI();
        }
        public static void RemoveItem(int position)
        {
                if (!contents.ContainsKey(position)){ return; }
            contents.Remove(position);

            UpdateUI();
        }
        public static void RemoveSelectedItem()
        {
                if (!contents.ContainsKey(selectedSlot)){ return; }
            contents.Remove(selectedSlot);

            UpdateUI();
        }
        public static bool QueryHasItem(soDATA_Item item)
        {
            return contents.ContainsValue(item);
        }
        public static bool QueryIsFull()
        {
            bool result = true;
            for (int i = 0; i < inventorySize.max; i++)
            {
                if (!contents.ContainsKey(i)){ result = false; }
            }
            return result;
        }
        public static void UpdateUI()
        {
            UI_Inventory InventoryUI = FindAnyObjectByType<UI_Inventory>();
            InventoryUI?.UpdateUI();
        }
        public static void SetSelectedSlot(int newSelectedSlot)
        {
            selectedSlot = newSelectedSlot;
            if (selectedSlot > inventorySize.max){ selectedSlot = inventorySize.max; }
            if (selectedSlot < inventorySize.min){ selectedSlot = inventorySize.min; }

            UpdateUI();
        }
        public static void UseItem(int slotPosition)
        {
                if (!contents.ContainsKey(slotPosition)){ return; }
            soDATA_Item itemUsed = contents[slotPosition];
                if (itemUsed.STAT_itemUseEffect == null){ return; }
            RemoveItem(slotPosition);
            itemUsed.STAT_itemUseEffect.Apply();
        }
        public static soDATA_Item GetSelectedItem()
        {
                if (!contents.ContainsKey(selectedSlot)){ return null; }
            return contents[selectedSlot];
        }
        public static soDATA_Item PopSelectedItem()
        {
                if (!contents.ContainsKey(selectedSlot)){ return null; }
            soDATA_Item selectedItem = contents[selectedSlot];
            RemoveItem(selectedSlot);
            return selectedItem;
        }
    #endregion

    #region DEBUGBUTTONS
        [Button]
        public void DEBUG_UpdateUI()
        {
            UpdateUI();
        }
        [Button]
        public void DEBUG_LogInventoryContents()
        {
            foreach (var item in contents)
            {
                Log(item.Value.name);
            }
        }
        [Button]
        public void DEBUG_AddContents()
        {
            foreach (var item in DEBUG_addContents)
            {
                AddItem(item);
            }

            UpdateUI();
        }
    #endregion
}