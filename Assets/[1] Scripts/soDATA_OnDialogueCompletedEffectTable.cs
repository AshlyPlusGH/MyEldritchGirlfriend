using System;
using Ash.SOEffectSystem;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu]
public class soDATA_OnDialogueCompletedEffectTable : ScriptableObject
{
    [SerializeField] private GenericDictionary<string,soDATA_Effect> soDATA_data = new();

    public GenericDictionary<string,soDATA_Effect> STAT_data => soDATA_data;
}