using UnityEngine;

[CreateAssetMenu]
public class soDATA_TimeSystemBalanceData : ScriptableObject
{
    [SerializeField] private float soDATA_nightLength;

    public float STAT_nightLength => soDATA_nightLength;
}