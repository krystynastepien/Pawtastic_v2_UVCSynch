using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
/// <summary>
/// Right page of the book: name, big icon and description of the selected item.
/// </summary>
public class InventoryItemDescription : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;
 
    public void OnInit()
    {
        ClearDescription();
    }
 
    public void SetDescription(ItemData item)
    {
        if (item == null)
        {
            ClearDescription();
            return;
        }
 
        Sprite sprite = item.GetBigIcon();
        _icon.gameObject.SetActive(true);
        _icon.sprite = sprite;
        _icon.color = sprite != null ? Color.white : new Color32(190, 190, 190, 255);
 
        _title.text = item.Name;
        _title.color = ItemRarityColors.Get(item.Rarity) == Color.white
            ? _title.color
            : ItemRarityColors.Get(item.Rarity);
        _description.text = item.Description;
    }
 
    public void ClearDescription()
    {
        _icon.gameObject.SetActive(false);
        _title.text = string.Empty;
        _description.text = string.Empty;
    }
}