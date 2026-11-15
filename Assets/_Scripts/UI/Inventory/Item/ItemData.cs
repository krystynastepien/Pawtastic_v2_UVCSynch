using UnityEngine;
using Pawtastic.Utils.Enums;

// Blueprint of an item (static data). One asset per item type.
[CreateAssetMenu(fileName = "ItemData", menuName = "Custom Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    #region Fields

    [Header("Identity")]
    [Tooltip("Unique, stable id (used by the database, later by save/load). Auto-filled")]
    public string Id;
    public Items ItemType;
    public ItemUse Action;
    public string Name;
    [TextArea] public string Description;
    [TextArea] public string OnUseDescription;
 
    [Header("Visuals")]
    [Tooltip("Small icon in inventory slot")]
    public Sprite Icon;
    [Tooltip("Big icon in description window. Falls back to Icon if empty.")]
    public Sprite BigIcon;
    public ItemRarity Rarity;

 
    [Header("Rules")]
    [Tooltip("Quest items live in the quest tab, don't take slots and can't be dropped.")]
    public bool IsQuestItem;
    [Tooltip("Can be dragged to hotbar slots 1-5.")]
    public bool IsHotBarUse;
    [Tooltip("Trade value (Shiny Objects). Rarity can scale it.")]
    public int Value;
    public int MaxStackSize;
 
    #endregion

 #region Methods
 
    /// <summary>
    /// Anything can be dropped except quest items.
    /// </summary>
    public bool CanDrop() => !IsQuestItem;
 
    public Sprite GetBigIcon() => BigIcon != null ? BigIcon : Icon;
 
    public void OnUse()
    {
        // TODO: apply effect (StatModifiers + Duration) based on Action
    }
 
    #endregion
 
    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(Id))
            Id = name;
        if (MaxStackSize < 1)
            MaxStackSize = 1;
        if (IsQuestItem)
            IsHotBarUse = false;
    }


}
