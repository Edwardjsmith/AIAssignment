using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inventory for the AI
/// </summary>
public class InventoryController : MonoBehaviour
{
    private const int _Capacity = 4;
    public int Capacity
    {
        get { return _Capacity; }
    }

    // The inventory
    private Dictionary<string, GameObject> _inventory = new Dictionary<string, GameObject>(_Capacity);
    public Dictionary<string, GameObject> Items
    {
        get { return _inventory; }
    }

    /// <summary>
    /// Adds an item to the inventory if there's enough room (max capacity is 'Constants.InventorySize')
    /// </summary>
    /// <param name="item">The invenroy item to add, as type 'IUsableItem'</param>
    /// <returns>true if the item was successfully added, false otherwise</returns>
    public bool AddItem(GameObject item)
    {
        if (_inventory.Count < _Capacity && item.GetComponent<Collectable>() != null)
        {
            if(!_inventory.ContainsKey(item.name))
            {
                _inventory.Add(item.name, item);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Retrieves an item from the inventory as a GameObject, does not remove it from the inventory
    /// </summary>
    /// <param name="itemName">The string representing the tag of the item to get e.g. "HealthKit"</param>
    /// <returns></returns>
    public GameObject GetItem(string itemName)
    {
        GameObject item;
        if (_inventory.TryGetValue(itemName, out item))
        {
            return item;
        }
        return null;
    }

    public bool RemoveItem(string itemName)
    {
        if (_inventory.ContainsKey(itemName))
        {
            _inventory.Remove(itemName);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if an item is stored in the inventory
    /// </summary>
    /// <param name="itemName">The string representing the tag of the item e.g. "HealthKit"</param>
    /// <returns></returns>
    public bool HasItem(string itemName)
    {
        return _inventory.ContainsKey(itemName);
    }
}
