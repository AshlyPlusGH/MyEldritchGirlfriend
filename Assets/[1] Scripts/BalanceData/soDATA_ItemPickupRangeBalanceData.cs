using UnityEngine;

[CreateAssetMenu]
public class soDATA_ItemPickupRangeBalanceData : ScriptableObject
{
    [SerializeField] private float soDATA_pickupRange;

    public float STAT_pickupRange => soDATA_pickupRange;
}