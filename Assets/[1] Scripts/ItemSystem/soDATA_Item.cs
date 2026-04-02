using Ash.SOEffectSystem;
using UnityEngine;

[CreateAssetMenu]
public class soDATA_Item : ScriptableObject
{
    [SerializeField] private Sprite soDATA_sprite;
    [SerializeField] private soDATA_Effect soDATA_itemUseEffect;

    public Sprite STAT_sprite => soDATA_sprite;
    public soDATA_Effect STAT_itemUseEffect => soDATA_itemUseEffect;
}