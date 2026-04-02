using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RitualCircle : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents + " at: " + name); }}

    [Space(10)]

    [SerializeField] private List<ItemSlotPhysical> sigilSlots = new();

    [SerializeField] private UnityEvent unityEventOnSigilAdded;
    [SerializeField] private UnityEvent unityEventOnAllSigilsAdded;

    void Awake(){ Setup(); }
    public void Setup()
    {
        foreach (var sigilSlot in sigilSlots)
        {
            sigilSlot.onItemPlaced += OnSigilAdded;
            sigilSlot.onItemRemoved += OnSigilRemoved;
        }
    }

    void OnSigilAdded()
    {
        bool allSigilsAdded = true;
        foreach (var sigilSlot in sigilSlots)
        {
            if (!sigilSlot.QueryContainsAllowedItem()){ allSigilsAdded = false; }
        }
        if (allSigilsAdded){ OnAllSigilsAdded(); }

        unityEventOnSigilAdded.Invoke();
    }
    void OnSigilRemoved(){}

    void OnAllSigilsAdded()
    {
        unityEventOnAllSigilsAdded.Invoke();
    }
}