---
tags:
  - game-design
  - items
  - unity
  - csharp
  - ui
  - inventory
---


### Faza 0: Decyzje projektowe (przed kodem)

- **Definicja vs instancja:** `ItemDefinition` (dane, np. ScriptableObject/JSON) i `ItemStack` (runtime: `ItemId` + `Count`).
- **Gdzie żyje flaga “to sell”:** na slocie/stacku, nie na definicji, bo to stan gracza.
- **Hotbar trzyma referencję, nie kopię:** wskazuje na item/slot w inventory.
- Wszystkie zmiany idą przez eventy (`OnSlotChanged`), UI nigdy nie modyfikuje danych bezpośrednio.

### Faza 1: Model danych

csharp

```csharp
public class ItemDefinition {
    public string Id;
    public int MaxStackSize = 1;
    public bool IsQuestItem;
    public bool IsHotBarUse;
    public bool CanDrop() => !IsQuestItem;
    public bool IsStackable => MaxStackSize > 1;
}

public class ItemStack {
    public ItemDefinition Item;
    public int Count;
    public bool MarkedToSell;
}
```

### Faza 2: Kontener (serce systemu)

`Inventory` ze slotami (`ItemStack?[]`) i metodami:

- `int TryAdd(item, count)` zwraca **resztę, która się nie zmieściła**. Najpierw dopełnia istniejące stacki, potem zajmuje puste sloty.
- `TryRemove`, `Swap`, `Move`, `Split`, `Merge`
- `Expand(int extraSlots)` do ulepszeń od NPC/itemów (tylko w górę)
- **Zakładka questowa:** osobny kontener bez limitu slotów. `TryAdd` kieruje tam przedmioty po `IsQuestItem`.

To jest największy kawałek logiki, więc **napisz do niego testy od razu**.

### Faza 3: Pickup z mapy

- `WorldItem` (komponent na mapie) trzyma `ItemStack`
- Interakcja/trigger woła `inventory.TryAdd`
- **Częściowy pickup:** jeśli zmieściła się tylko część, na mapie zostaje reszta
- Pełny ekwipunek daje feedback (“Brak miejsca”), a quest itemy zawsze się zbierają

### Faza 4: Dropping

- Najpierw `CanDrop()`; dla quest itemów blokada plus komunikat
- Spawn `WorldItem` przy graczu, z opcją upuszczenia części stacka
- Co z flagą “to sell” przy dropie? Po prostu ją zeruj.

### Faza 5: UI

- Siatka slotów podpięta pod eventy kontenera
- Zakładki: Items / Quest
- Drag & drop (przeniesienie, swap, split ze Shiftem)
- Tooltip, licznik stacka, oznaczenie “to sell”

Jeśli UI robisz w Unity UI Toolkit/uGUI, użyj wzorca MVP: `InventoryPresenter` łączy model z widokiem.

### Faza 6: Hotbar (sloty 1-5)

- Przyjmuje tylko itemy z `IsHotBarUse`
- Drag z inventory tworzy powiązanie ze slotem hotbara
- Obsłuż przypadki: item się skończył (slot pusty), item wyrzucony, ten sam item w dwóch slotach hotbara
- Klawisze 1-5 uruchamiają użycie

### Faza 7: Sprzedaż

- Toggle “to sell” na stacku
- `SellAllMarked(shop)` w pętli po slotach, liczy sumę, usuwa stacki, dodaje walutę
- Okno potwierdzenia ze sumą
- Blokady: quest itemy nie mogą być oznaczone, a item “to sell” nie może trafić na hotbar (albo flaga zdejmuje się automatycznie, zdecyduj jedną zasadę)

### Faza 8: Ulepszanie slotów

NPC lub item wywołuje `Expand(n)`. Dodaj limit maksymalny i event dla UI, żeby siatka się przebudowała.

### Faza 9: Zapis i wczytywanie

Serializuj `ItemId + Count + MarkedToSell + pozycja slotu` oraz aktualną pojemność. Przy wczytaniu rozwiązuj `ItemId` przez bazę definicji. Dodaj wersję zapisu na przyszłość.

### Faza 10: Szlify i edge case’y

- Dodanie stacka większego niż `MaxStackSize`
- Dodanie 0 lub liczby ujemnej
- Zmniejszenie `MaxStackSize` w definicji po wcześniejszym zapisie
- Zapełniony ekwipunek i pickup wielu itemów naraz
- Dźwięki, animacje pickupu, powiadomienia “+3 Potion”

### Proponowana kolejność (MVP → pełne)

1. Fazy 1-2 + testy (bez żadnego UI)
2. Faza 3 + prosta lista tekstowa do debugowania
3. Fazy 4-5 (drop + normalne UI)
4. Fazy 6-7 (hotbar, sprzedaż)
5. Fazy 8-10