---
tags:
  - unity
  - ui
  - textmeshpro
  - inventory
---

# Inventory: Unity UI Notes

> Back to [[Inventory Index]] · UI design: [[Inventory - Rules and UI]]

## UI & EventSystem

**Rule:** only UI elements react to the EventSystem, so an **Image component is mandatory** on anything that should receive pointer events.

Source: https://discussions.unity.com/t/event-triggers-event-systems-help/157973

**Checklist when UI clicks/hovers don't work**
- [ ] An `EventSystem` exists in the scene (Hierarchy → UI → Event System).
- [ ] The Canvas has a `GraphicRaycaster`.
- [ ] The object has a `Graphic` (Image / RawImage / TMP text) with **Raycast Target** enabled. An invisible hit area can be an Image with alpha 0.
- [ ] Nothing with Raycast Target sits on top and blocks it.
- [ ] Scripts implement `IPointerClickHandler`, `IPointerEnterHandler`, etc., or the object has an `EventTrigger`.
- [ ] Non-UI objects (world sprites/3D) need a `Collider` + `PhysicsRaycaster` on the camera instead.

## Fonts, Colors & Materials

**Note:** to use the *same font* with *different colors/materials*, you have to **make separate font assets** (or material presets) for each variant.

- With TextMeshPro, duplicate the **Material Preset** of the font asset (outline, glow, underlay) for each style.
- For a plain color change, use the text's `Color` / `Vertex Color` instead of a new material.
- Name presets by use, e.g. `Pixel_Title_Gold`, `Pixel_Body_White`.








## Side notes

Obraz który jest tłem i powinien mieć elementy które się nie rozciągają, musi mieć mniejszy border w spirte editorze, np, nie chcę aby się rozciągały złote rogi

![[Pasted image 20261025125110.png]]
Borders: 45
![[Pasted image 20261025125513.png]]



