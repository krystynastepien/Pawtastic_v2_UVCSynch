---
onenote-id: c8501254-6b17-4be6-9789-010866b9c37d:1
tags: [unity, tips, scriptableobject, steam]
---

# Tips and Tricks

> Related: [[Code]], [[programming]]

## `[CreateAssetMenu]`

An attribute that adds a custom entry to Unity's **Create** menu, so you can make ScriptableObject assets from the Project window.

```csharp
[CreateAssetMenu(fileName = "LoggerConfig", menuName = "Config/Logger Config")]
public class LoggerConfig : ScriptableObject { }
```

| Parameter | Meaning | Example |
|---|---|---|
| `fileName` | The **default name** of the new asset (it can be renamed after creation) | Creates `LoggerConfig.asset` |
| `menuName` | The **path in the Create menu**. `/` makes submenus | `"Config/Logger Config"` → **Create → Config → Logger Config** |
| `order` *(optional)* | Position within the menu | `order = 1` |

Tip: put the attribute directly above the class and keep one asset class per file, named like the class.

---

## Steam Resolutions

Most used resolutions on Steam (use for testing and UI scaling):

![[TIPS AND TRICKS image.png]]
![[TIPS AND TRICKS image 1.png]]

Tip: check the current numbers in the Steam Hardware & Software Survey (https://store.steampowered.com/hwsurvey) and test your UI at 1920x1080, 2560x1440 and 1366x768 at minimum.

---

## Scaling Images / UI

*(Note was only a heading, so these are starter tips to fill in.)*
- Use a **Canvas Scaler** set to *Scale With Screen Size* and pick one reference resolution (e.g. 1920x1080).
- Set **Match** between Width and Height (0.5 is a common default), or use Expand/Shrink for fixed layouts.
- Anchor UI elements to the correct corners/edges instead of absolute positions.
- For pixel art: set the sprite's *Filter Mode* to **Point (no filter)** and *Compression* to **None**, and use *Pixel Perfect Camera*.
- Use **9-slice** (sprite border) for panels that need to scale without distorting corners.
