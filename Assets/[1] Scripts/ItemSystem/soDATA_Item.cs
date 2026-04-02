using Ash.SOEffectSystem;
using UnityEngine;

[CreateAssetMenu]
public class soDATA_Item : ScriptableObject
{
    [SerializeField] private Sprite soDATA_sprite;
    [SerializeField] private soDATA_Effect soDATA_itemUseEffect;
    [SerializeField] private GameObject soDATA_itemModel;
    [SerializeField] private bool soDATA_collectible;

    public Sprite STAT_sprite => soDATA_sprite;
    public soDATA_Effect STAT_itemUseEffect => soDATA_itemUseEffect;
    public GameObject STAT_itemModel => soDATA_itemModel;
    public bool STAT_collectible => soDATA_collectible;
}