using UnityEditor.U2D.Aseprite;
using UnityEngine;
 
/// <summary>
/// Item lying in the world, the player picks it up by walking into the trigger
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemData _item;
    [SerializeField, Min(1)] private int _quantity = 1; //quantity (how much) of an item is picked up

    private InventoryController _inventory;
 
    private void Reset()
    {
        // make sure the collider is a trigger when the component is added
        GetComponent<Collider2D>().isTrigger = true;
    }
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_item == null || !other.CompareTag(EditorObjectTag.Player)) return;
 
        // TODO: replace with your manager (e.g. PlayerManager.Instance) once it exposes the inventory
        if (_inventory == null)
            //_inventory = FindFirstObjectByType<InventoryController>(FindObjectsInactive.Include);
        if (_inventory == null) return;
 
        int notAdded = _inventory.TryAddItem(_item, _quantity);
        if (notAdded <= 0)
            Destroy(gameObject);
        else
            _quantity = notAdded; // inventory full: leave the rest in the world
    }
}