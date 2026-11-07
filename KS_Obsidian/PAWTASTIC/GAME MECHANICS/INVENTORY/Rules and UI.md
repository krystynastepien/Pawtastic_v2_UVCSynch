---
tags:
  - game-design
  - ui
  - inventory
---

# Inventory: Rules and UI

> Back to [[Inventory Index]]

## Core Rules

| Rule | Detail |
|---|---|
| Slot limit | The pocket dimension has limited slots, upgradeable via NPCs / items |
| Dropping | Anything can be dropped **except quest items** (`CanDrop()`) |
| Quest items | Live in a **separate quest tab** and **do not take slots** |
| Stacking | Stackable items share a slot up to `MaxStackSize` |
| Selling | Items can be marked **"to sell"** and sold in bulk later |
| Hotbar | Items flagged `IsHotBarUse` can be dragged to hotbar slots 1-5 |
| Durability | **Not used** (decided against) |

**Why a slot limit?**
- Forces choices (what to keep?).
- Gives a reason to trade and sell.
- Stops hoarding and a messy inventory.
- Stays immersive (pocket dimension, upgradeable by NPCs or items).

## UI & Interaction

- Each item shows **Use / Drop** buttons.
- **Mark as "To Sell"** toggle (icon or tint on the slot), with a **"Sell all marked"** button at vendors.
- **Hotbar (1-5):** drag consumables and tools onto it for quick use.
- **Tabs:** Items | Equipment | Quest/Story | Gallery.
- **Rarity** is shown as a slot border color.
- **Two icons per item:** small (slot button) and big (description window).
- **Equipment panel** with slots for Collar, Ribbon/Hat and Paw Gear (see [[Inventory - Items Equipment]]).
- Suggested extras: sort/filter buttons, "new item" badge, confirm dialog when dropping rare items.

## Related
- Data fields behind these rules: [[Inventory - Data Model]]
- Making the UI respond to clicks: [[Inventory - Unity UI Notes]]
