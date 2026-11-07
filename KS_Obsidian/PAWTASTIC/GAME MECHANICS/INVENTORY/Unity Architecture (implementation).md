---
onenote-id: 3cebc516-809b-421e-acb1-2ce8dd33f31e:1
tags:
  - unity
  - csharp
  - scriptableobject
  - inventory
---

# Inventory: Unity Architecture

> Back to [[Inventory Index]] · Fields: [[Inventory - Data Model]] · See also [[TIPS AND TRICKS]], [[programming]]

## Item data = ScriptableObjects, not prefabs
- Prefabs are for GameObjects in the scene/world.
- Inventory items are just **data** (name, description, sprite, type), which is exactly what ScriptableObjects are for.

```csharp
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public HUD_Item ItemType;
    public string ItemName;
    [TextArea] public string Description;
    public Sprite Icon;
    public int MaxStackSize = 1;
}
```

## Class responsibilities

| Class | Kind | Role |
|---|---|---|
| `ItemData` | ScriptableObject asset | The **blueprint** (static data) |
| `ItemPickup` | MonoBehaviour (world) | Holds an `ItemData`, lets the player pick it up |
| `InventorySlotData` | Plain class / struct (runtime) | Slot state: `ItemData` + quantity |
| `InventoryItemSlot` | UI component | Displays an `InventorySlotData` |
| `InventoryController` | UI controller | Owns slots, handles open/close, add/remove, selection |
| `InventoryItemDescription` | UI component | Shows the selected item's name, icon and text |

**Why this split?** Never store runtime values (like quantity) on the ScriptableObject: it's shared by every holder and changes persist in the editor. Keep runtime state in `InventorySlotData`.

```csharp
[System.Serializable]
public struct InventorySlotData
{
    public ItemData Item;   // null = empty slot
    public int Quantity;

    public bool IsEmpty => Item == null || Quantity <= 0;
}
```

## Implementation notes
- **Definition vs instance:** `ItemData` (static) plus a lightweight `ItemStack { ItemData data; int count; bool markedForSale; }` as the runtime/save object.
- **Containers:** `Inventory` (slot list, with capacity), `QuestInventory` (unlimited, no drop) and `Equipment` (slot to item dictionary).
- **Events:** `OnInventoryChanged`, `OnItemUsed`, `OnEquipChanged`, so the UI and stat system react without polling.
- **Effects:** consumables apply `StatModifiers` for `Duration` through a buff manager that also handles stacking and expiry.
- **Saving:** serialize `{ id, count, markedForSale }` per slot, plus capacity upgrades.
- **Capacity upgrades:** NPC / item adds slots (`+N`) and persists in the save.
- **Bulk sell:** vendor UI sums `Value * count` for all marked stacks, with a confirm dialog.

## Project files

Location: `D:\Evenes\programowanie\Unity project\Pawtastic\Pawtastic v2\Assets\_Scripts\UI\Inventory\`

- `InventoryController.cs`
- `InventoryItemDescription.cs`
- `InventoryItemSlot.cs`
