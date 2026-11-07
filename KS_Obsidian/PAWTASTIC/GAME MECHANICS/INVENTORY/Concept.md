---
tags:
  - game-design
  - items
  - unity
  - csharp
  - "#references"
  - ui
  - inventory
---
``
# Inventory System

> Source: Discord brainstorm (https://discord.com/channels/1064579448239816835/1064579449330356386), reorganized and expanded.

## Contents - headers
 [[#1. Concept & Design]]
 [[#^inv-rules|2. Core Rules]]
 [[#^ui-link|3. UI & Interaction]]
 [[#4. Item Catalogue]]
	[[#4.1 Items Consumables and Currency Consumables]]
	[[#4.2 Items Equipment Equipment]]
	[[#4.3 Items Consumables and Currency Currency & Trade]]
	[[#^inv-special-items|4.4 Special / Utility Items]]
	[[#^inv-story-items|4.5 Quest / Story / Lore Items]]
 [[#^inv-data-model|5. Data Model]]
 [[#^inv-notes|6. Implementation notes]]
 [[#7. Open Questions & To Consider]]
 [[#^inv-ref|8. Reference Images]]
 
---

## 1. Concept & Design

**Fiction:** The cat carries everything in a **magic pocket dimension**, granted by a magic raven or a witch.

**Pillars**
- **Theme first.** The player is a cat. Items should fit the world: collars, bows, scraps, shiny trinkets, not potions and swords.
- **Meaningful choices.** Limited slots force the player to decide what to keep.
- **Reasons to trade.** A full pocket pushes the player to sell or trade, which also drives the economy (shiny objects, favors).
- **No hoarding or mess.** A tidy inventory is easier to read and to build UI for.
- **Immersive.** Every mechanic has an in-world explanation (pocket dimension, witch upgrades, etc.).

---

## [[Rules and UI|2. Core Rules]]^inv-rules

| Rule | Detail |
|---|---|
| Slot limit | The pocket dimension has limited slots, upgradeable via NPCs / items |
| Dropping | Anything can be dropped **except quest items** (`CanDrop()`) |
| Quest items | Live in a **separate quest tab** and **do not take slots** |
| Stacking | Stackable items share a slot up to `MaxStackSize` |
| Selling | Items can be marked **"to sell"** and sold in bulk later |
| Hotbar | Items flagged `IsHotBarUse` can be dragged to hotbar slots 1-5 |
| Durability | **Not used** (decided against) |

---

## 3. [[Rules and UI|UI & Interaction]]^ui-link

- Each item shows **Use / Drop** buttons.
- **Mark as "To Sell"** toggle (shown as an icon or tint on the slot), with a **"Sell all marked"** button at vendors.
- **Hotbar (1-5):** drag consumables and tools onto it for quick use.
- **Tabs:** Items | Equipment | Quest/Story | Gallery.
- **Rarity** is shown as a slot border color.
- **Two icons per item:** small (slot button) and big (description window).
- **Equipment panel** with slots for Collar, Hat and Paw Gear (see [[#^Slot-types]])
-
- Suggested extras: sort/filter buttons, "new item" badge, confirm dialog when dropping rare items.

---

## 4. Item Catalogue

### 4.1  [[Items Consumables and Currency|Consumables]]


| Item                            | Effect                            | Rarity        | Notes                                          |
| ------------------------------- | --------------------------------- | ------------- | ---------------------------------------------- |
| Milk / Water                    | Small heal                        | Common        |                                                |
| Raw Fish                        | Normal heal                       | Common        |                                                |
| Tuna Can                        | Large heal + temporary stat boost | **Very rare** |                                                |
| "Questionable" Garbage / Scraps | High heal                         | Uncommon      | Chance to make the cat **sick** (slower speed) |
| Catnip Bag / Herb               | Temporary speed boost             | Uncommon      |                                                |
| Cardboard Box                   | Temporary hiding spot (stealth)   | Uncommon      | Consumable placeable                           |

### 4.2 [[Items Equipment|Equipment]]

<a id="equipment-slots"></a>
**[[Slots]]:** `Collar`, `Ribbon/Hat`, `PawGear` (new slot idea). ^Slot-types

**Collars / Tags**: affect cat stats and maybe appearance a little.

| Item | Effect |
|---|---|
| Bell Collar | Enemies detect you more easily (bigger detect area), but **better loot drops** |
| Spiked Collar | +Attack, intimidation bonus |
| Reflective Collar | +Defense vs birds / aerial attacks |
| Flea Collar | Immunity to poison / debuff effects |

*Also planned:* small buffing ties, bows and collars for the cat.

**Paw Gear**

| Item | Effect |
|---|---|
| Climbing Claws | Scale certain walls / ice |
| Soft Paw Wraps | Extra stealth |
| *(idea)* Water-walking item | Walk on water (name TBD) |

### 4.3 [[Items Consumables and Currency|Currency & Trade]]

- **Shiny Objects** are the currency used to trade with other animals (e.g. the magpie).
  - Examples: marbles, rings, foil balls, bottle caps, buttons, coins.
- **Favors:** tokens representing debts from NPCs (a second, non-material currency).
- Items have a `Value` for trading, and rarity can scale prices.

### 4.4 [[Items Quest and Special|Special / Utility Items]]^inv-special-items

| Item | Effect | Open question |
|---|---|---|
| Nine Lives Charm | Auto-revive once (very rare) | Item or automatic skill? |
| Moonstone Fragment | Reveals spirit animals in the woods | |
| Witch's Blessing | Fast travel to discovered checkpoints via portals | Maybe a teleport stone |

### 4.5 [[Items Quest and Special|Quest / Story / Lore Items]]^inv-story-items

> These go in the separate quest tab and **can't be dropped**.

| Item                            | Purpose                                                                                       |
| ------------------------------- | --------------------------------------------------------------------------------------------- |
| **Memory Echoes**               | Collectible memories that play cutscenes                                                      |
| **Scent Vial**                  | Contains the family's scent, gets stronger near the city (could be used **instead of a map**) |
| **Torn Map Fragment**           | Helps navigate the neighborhood / city                                                        |
| **Cat's Old Collar Tag**        | Original collar with the family address                                                       |
| **Lost Collar Tag**             | Side-quest item for the dog on the farm                                                       |
| **Vet Certificate** (from farm) | Proof the cat is safe, needed for the city. *How to show it?*                                 |
| **Keys**                        | For doors or trash cans                                                                       |
| **Badges**                      | Awarded for completing tasks or exploring areas                                               |
| **Favors**                      | NPC debt tokens (also see Currency)                                                           |
| **Gallery items**               | Pictures / items from animals met in the world, plus world photos                             |

**Vet Certificate idea:** since it's a barn, show it as a **clip on the ear**.
- Optional side quest: if the cat has never been to a vet before the family left, the player must **acquire (steal) the clip**.
- This could become a fun **dungeon / stealth sequence in the vet office**.

---

## [[Data Model|5. Data Model (C#)]]^inv-data-model

### Enums

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

### Item class

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

### To consider adding to the class
- `RequiredLevel`
- `IsKeyItem`
- `IsDiscovered` (photo gallery tracking / teleport points)

### Id scheme (suggestion)
Use stable, readable string ids: `category.name`, e.g. `consumable.tuna_can`, `collar.bell`, `quest.vet_certificate`. Stable ids keep save files and localization safe when items are renamed.

---

## 6. [[Unity Architecture (implementation)|Implementation Notes]]^inv-notes

- **Definition vs instance:** In Unity, make `ItemData` a `ScriptableObject` (static: name, icons, flags, stats) and keep a lightweight `ItemStack { ItemData data; int count; bool markedForSale; }` as the runtime/save object. This avoids duplicating data across stacks and keeps `CurrentStackCount` out of the shared asset.
- **Containers:** `Inventory` (slot list, with capacity), `QuestInventory` (unlimited, no drop) and `Equipment` (slot to item dictionary).
- **Events:** `OnInventoryChanged`, `OnItemUsed`, `OnEquipChanged`, so the UI and stat system react without polling.
- **Effects:** consumables apply `StatModifiers` for `Duration` through a buff manager that also handles stacking and expiry.
- **Saving:** serialize `{ id, count, markedForSale }` per slot, plus capacity upgrades.
- **Capacity upgrades:** NPC / item adds slots (`+N`) and persists in the save.
- **Bulk sell:** vendor UI sums `Value * count` for all marked stacks, with a confirm dialog.

---

## 7. Open Questions & To Consider

- [ ] How is the **Vet Certificate** shown to NPCs (ear clip, tag, scent)?
- [ ] **Scent Vial** as a map replacement: is it a compass/meter or a tracking minigame?
- [ ] **Nine Lives Charm:** consumable item or passive skill?
- [ ] Water-walking item: what is it and how rare?
- [ ] Should **Bell Collar** loot bonus scale with enemy detection?
- [ ] How many starting slots and how many upgrades?
- [ ] Is the **Gallery** a separate system from the inventory (photo mode, collectible per animal)?
- [ ] Do sick-status effects from scraps stack with other debuffs (Flea Collar immunity)?
- [ ] Does `RequiredLevel` fit this game, or is it unnecessary?

---

## [[Inventory - All images|References]]^inv-ref

> The screenshots were not categorized (the order matches the original note). Consider renaming them descriptively, e.g. `inventory-ui-grid.jpg`, and grouping them by topic.

![[Inventory image.jpg]]
![[Inventory image 1.jpg]]
![[Inventory image 2.jpg]]
![[Inventory image 3.jpg]]
![[Inventory image 4.jpg]]
![[Inventory image 5.jpg]]
![[Inventory image 6.jpg]]
![[Inventory image 7.jpg]]
![[Inventory image 8.jpg]]
![[Inventory image 9.jpg]]
![[Inventory image 10.jpg]]
![[Inventory image 11.jpg]]
![[Inventory image 12.jpg]]
![[Inventory image 13.jpg]]
![[Inventory image 14.jpg]]
![[Inventory image.png]]
![[Inventory image 1.png]]
![[Inventory image 2.png]]
![[Inventory image 3.png]]
![[Inventory image 4.png]]
![[Inventory image 5.png]]
![[Inventory image 6.png]]
![[Inventory image 7.png]]
![[Inventory image 8.png]]
![[Inventory image 9.png]]
![[Inventory image 10.png]]
![[Inventory image 11.png]]
![[Inventory image 12.png]]
![[Inventory image 13.png]]
![[Inventory image 14.png]]
![[Inventory image 15.png]]
