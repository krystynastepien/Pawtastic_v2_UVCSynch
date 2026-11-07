---
tags:
  - game-design
  - items
  - unity
  - csharp
  - ui
  - inventory
---

## 1. Design

 **Warstwę widoku**: item slot w inventory, panel opisu, kontroler tworzący sloty, kontener na itemSlots. 
 
 
 Brakuje **modelu danych**: teraz slot sam trzyma `ItemData`, a ilość istnieje tylko jako tekst w liczniku. Następny krok to fazy 1-2 z roadmapy, czyli czyste C# bez `MonoBehaviour`, a potem przerobienie `InventoryController` na prezentera, który tylko odzwierciedla model.


# 2. Proof of concept
- **Sterowanie i UI:** Ekwipunek ma się otwierać i zamykać za pomocą przypisanego klawisza (np. I lub Tab).
    
- **Renderowanie slotów:** Interfejs wizualnie pokazuje wszystkie sloty na itemki, generując je w formie siatki (np. 4x4).
    
- **Wyświetlanie przedmiotów:** W slotach prezentowane są posiadane przez gracza przedmioty, wraz z renderowaniem ich ikon oraz opisów (w panelu podglądu obok ekwipunku).
    
- **Zbieranie przedmiotów:** Mechanika podnoszenia itemków ze świata (poprzez kolizję z graczem) i automatyczne dodawanie ich do pierwszego wolnego slotu.
    
- **Usuwanie i zużywanie:** Opcja wyrzucania/usuwania przedmiotów ze slotów (co zwalnia miejsce) oraz logika konsumpcji (np. użycie mikstury leczącej, która odejmuje ilość lub usuwa przedmiot).
    
- **Automatyczne sortowanie:** Po zużyciu lub usunięciu przedmiotu następuje natychmiastowe uporządkowanie ekwipunku (przesunięcie pozostałych elementów w stronę początku listy), aby uniknąć pustych przerw pomiędzy slotami.