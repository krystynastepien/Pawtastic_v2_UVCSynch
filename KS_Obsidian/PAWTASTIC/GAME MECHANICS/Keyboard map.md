---
onenote-id: d48c99c2-b946-48d0-bbf3-cb4c43ea8eae:1
---
I - inventory

public void Update()

{

if (Input.GetKey (KeyCode.I)) {

if (_inventoryHUDCanvas.activeSelf)

{

Hide(UIElement.InventoryUI);

}

else

{

Show(UIElement.InventoryUI);

}

}

}

D:\Evenes\programowanie\Unity project\Pawtastic\Pawtastic v2\Assets\_Scripts\Managers\HUDManager.cs