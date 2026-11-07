---
onenote-id: 3cebc516-809b-421e-acb1-2ce8dd33f31e:1
tags:
  - unity
  - csharp
  - ui
  - scriptableobject
  - inventory
---

# Code Notes (Unity)

> Related: [[Tips and tricks]], [[Programming]], [[Concept]]

## Contents
1. [UI & EventSystem](#1-ui--eventsystem)
2. [Fonts, Colors & Materials](#2-fonts-colors--materials)
3. [Item Architecture (ScriptableObjects)](#3-item-architecture-scriptableobjects)
4. [Item Examples](#4-item-examples)
5. [Project Files](#5-project-files)
6. [Screenshots](#6-screenshots)

---

## 1. UI & EventSystem

**Rule:** only UI elements react to the EventSystem, so an **Image component is mandatory** on anything that should receive pointer events.

Source: https://discussions.unity.com/t/event-triggers-event-systems-help/157973

**Checklist when UI clicks/hovers don't work**
- [ ] An `EventSystem` exists in the scene (Hierarchy → UI → Event System).
- [ ] The Canvas has a `GraphicRaycaster`.
- [ ] The object has a `Graphic` (Image / RawImage / TMP text) with **Raycast Target** enabled. An invisible hit area can be an Image with alpha 0.
- [ ] Nothing with Raycast Target sits on top and blocks it.
- [ ] Scripts implement `IPointerClickHandler`, `IPointerEnterHandler`, etc., or the object has an `EventTrigger`.
- [ ] Non-UI objects (world sprites/3D) need a `Collider` + `PhysicsRaycaster` on the camera instead.

---

## 2. Fonts, Colors & Materials

**Note:** to use the *same font* with *different colors/materials*, you have to **make separate font assets** (or material presets) for each variant.

Tips:
- With TextMeshPro, duplicate the **Material Preset** of the font asset (outline, glow, underlay) for each style.
- For a plain color change, use the text's `Color` / `Vertex Color` instead of a new material.
- Name presets by use, e.g. `Pixel_Title_Gold`, `Pixel_Body_White`.

---

## 3. Item Architecture (ScriptableObjects)

**Item data = ScriptableObjects, not prefabs.**
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

**Class responsibilities**

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

See [[Concept]] for the full item class design and [[Tips and tricks]] for `[CreateAssetMenu]`.

---

## 4. Item Examples

| Item | Description | Effect |
|---|---|---|
| **Badge** | Toy badge found in the wild. It has a ripped ribbon. | +1 Attack, +1 Defense |
| **Fish** | Can be found in rivers and trash cans. Smells weird. | Heals 2 hearts |
| **Key** | It looks very old. | Opens a certain gate |

---

## 5. Project Files

Location: `D:\Evenes\programowanie\Unity project\Pawtastic\Pawtastic v2\Assets\_Scripts\UI\Inventory\`

- `InventoryController.cs`
- `InventoryItemDescription.cs`
- `InventoryItemSlot.cs`

---

## 6. Screenshots

![[Code image.png]]
![[Code image 1.png]]
![[Code image 2.png]]
![[Code image 3.png]]
![[Code image 4.png]]
![[Code image 5.png]]
![[Code image 6.png]]
![[Code image 7.png]]
