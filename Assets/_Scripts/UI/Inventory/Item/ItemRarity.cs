using UnityEngine;
 
public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    SuperRare,
    SuperSuperRare
}
 
/// <summary>
/// Border tint per rarity. Common = white = frame keeps its original look.
/// </summary>
public static class ItemRarityColors
{
    public static Color Get(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Common:
                return new Color32(255, 255, 255, 255); // white
            case ItemRarity.Uncommon:
                return new Color32(120, 200, 120, 255); // light blue
            case ItemRarity.Rare:
                return new Color32(90, 172, 0, 255);  // green
            case ItemRarity.SuperRare:
                return new Color32(255, 156, 249, 255);  // pink
            case ItemRarity.SuperSuperRare:
                return new Color32(190, 120, 230, 255);  // lawenda
            default:
                return Color.white;
        }
    }
}