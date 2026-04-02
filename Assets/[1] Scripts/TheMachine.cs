using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TheMachine : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents + " at: " + name); }}

    [Space(10)]

    [SerializeField] private List<ItemSlotPhysical> cogSlots = new();

    [SerializeField] private UnityEvent unityEventOnCogAdded;
    [SerializeField] private UnityEvent unityEventOnAllCogsAdded;

    void Awake(){ Setup(); }
    public void Setup()
    {
        foreach (var cogSlot in cogSlots)
        {
            cogSlot.onItemPlaced += OnCogAdded;
            cogSlot.onItemRemoved += OnCogRemoved;
        }
    }

    void OnCogAdded()
    {
        bool allCogsAdded = true;
        foreach (var cogSlot in cogSlots)
        {
            if (!cogSlot.QueryContainsAllowedItem()){ allCogsAdded = false; }
        }
        if (allCogsAdded){ OnAllCogsAdded(); }

        unityEventOnCogAdded.Invoke();
    }
    void OnCogRemoved(){}

    void OnAllCogsAdded()
    {
        unityEventOnAllCogsAdded.Invoke();
    }
}