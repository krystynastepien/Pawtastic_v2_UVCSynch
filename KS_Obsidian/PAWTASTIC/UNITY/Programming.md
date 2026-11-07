---
onenote-id: abd34f80-c7f8-4a7c-a310-4b04f0c12ecb:1
tags: [csharp, unity, fundamentals]
---

# Programming Notes

> Related: [[Code]], [[TIPS AND TRICKS]]

## Class vs Struct

| | **struct** | **class** |
|---|---|---|
| Type | Value type | Reference type |
| Can be `null` | **No.** It is always initialized with default data (zeros / nulls inside) | **Yes**, unless it is serialized (visible in the Inspector) |
| Copy on assign | Copies the whole value | Copies the reference (both point to one object) |
| Memory | Usually on the stack or inline in its container | On the heap (garbage collected) |
| Inheritance | No (interfaces only) | Yes |
| Best for | Small, simple data (vectors, slots, ids) | Larger objects, shared state, behavior |

### Unity specifics
- A **serialized class** field (`[System.Serializable]`, public or `[SerializeField]`) is created automatically by the Inspector, so it is *not* null in practice. The same field in a non-serialized context can be null.
- A plain `class` that isn't `[Serializable]` won't show in the Inspector.
- `MonoBehaviour` and `ScriptableObject` are classes, but create them with `AddComponent` / `ScriptableObject.CreateInstance`, never `new`.
- A struct field that holds references (e.g. `ItemData Item`) can still contain a null reference inside it.

### Quick example

```csharp
public struct SlotStruct { public int Count; }
public class SlotClass  { public int Count; }

SlotStruct s = default;   // valid, Count = 0
SlotClass  c = null;      // valid; c.Count would throw NullReferenceException

var s2 = s; s2.Count = 5; // s.Count is still 0 (copy)
var c2 = new SlotClass(); var c3 = c2; c3.Count = 5; // c2.Count is also 5 (same object)
```

### Rule of thumb
Use a **struct** for small, immutable-ish data that is copied around (e.g. an inventory slot record). Use a **class** when identity, shared state or inheritance matters.

![[programming image.png]]
