using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite itemImage;
    public int itemPrice;
    public string itemDescription;
}
