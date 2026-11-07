---
tags:
  - game-design
  - items
  - consumables
  - currency
  - inventory
---

# Items: Consumables and Currency

> Back to [[Inventory Index]]

## Consumables

| Item | Effect | Rarity | Notes |
|---|---|---|---|
| Milk / Water | Small heal | Common | |
| Raw Fish | Normal heal | Common | Found in rivers and trash cans |
| Tuna Can | Large heal + temporary stat boost | **Very rare** | |
| "Questionable" Garbage / Scraps | High heal | Uncommon | Chance to make the cat **sick** (slower speed) |
| Catnip Bag / Herb | Temporary speed boost | Uncommon | |
| Cardboard Box | Temporary hiding spot (stealth) | Uncommon | Consumable placeable |

Temporary effects use `StatModifiers` + `Duration` (see [[Inventory - Data Model]]).

## Currency & Trade

- **Shiny Objects** are the currency used to trade with other animals (e.g. the magpie).
  - Examples: marbles, rings, foil balls, bottle caps, buttons, coins.
- **Favors:** tokens representing debts from NPCs (a second, non-material currency).
- Items have a `Value` for trading, and rarity can scale prices.
- Items can be marked **"to sell"** and sold in bulk (see [[Inventory - Rules and UI]]).
