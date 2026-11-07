---
tags:
  - csharp
  - unity
  - data-model
  - inventory
---

# Inventory: Data Model (C#)

> Back to [[Inventory Index]] · Unity implementation: [[Inventory - Unity Architecture]]

## Enums

```csharp
public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Legendary // quest items
}

public enum EquipmentSlot
{
    None,
    Collar,
    RibbonHat,
    PawGear
}

public enum StatType
{
    MaxHealth,
    Attack,
    Defense,
    Speed,
    Stealth,
    DetectionRadius,
    LootBonus
}
```

## Item class (design draft)

```csharp
public class Item
{
    // Identity
    public string Id;            // see "Id scheme" below
    public string Name;
    public string Description;
    public Sprite IconSmall;     // slot button
    public Sprite IconBig;       // description window

    // Type flags
    public bool IsQuestItem;
    public bool IsConsumable;
    public bool IsEquippable;
    public bool IsStackable;
    public bool IsTradeable;
    public bool IsHotBarUse;     // can be dragged to hotbar 1-5

    // Stack management
    public int MaxStackSize = 1;
    public int CurrentStackCount = 1;

    // Value & rarity
    public int Value;            // for trading
    public ItemRarity Rarity;    // border color

    // Equipment / effects
    public EquipmentSlot EquipmentSlot = EquipmentSlot.None;
    public Dictionary<StatType, float> StatModifiers = new();
    public float Duration;       // seconds, for buffs

    // Player state
    public bool IsMarkedForSale;

    public bool CanDrop() => !IsQuestItem; // quest items can't be dropped
}
```

> In Unity, split this into static data (`ItemData` ScriptableObject) and runtime state (`InventorySlotData`), so `CurrentStackCount` and `IsMarkedForSale` don't live on a shared asset. See [[Inventory - Unity Architecture]].

## Field groups

| Group | Fields |
|---|---|
| Identity | `Id`, `Name`, `Description`, `IconSmall`, `IconBig` |
| Type flags | `IsQuestItem`, `IsConsumable`, `IsEquippable`, `IsStackable`, `IsTradeable`, `IsHotBarUse` |
| Stacking | `MaxStackSize` (=1), `CurrentStackCount` (=1) |
| Value & rarity | `Value`, `Rarity` |
| Equipment / buffs | `EquipmentSlot`, `StatModifiers`, `Duration` |

## To consider adding
- `RequiredLevel`
- `IsKeyItem`
- `IsDiscovered` (photo gallery tracking / teleport points)
- `HasDurability`: decided **no**.

## Id scheme (suggestion)
Use stable, readable string ids: `category.name`, e.g. `consumable.tuna_can`, `collar.bell`, `quest.vet_certificate`. Stable ids keep save files and localization safe when items are renamed.
