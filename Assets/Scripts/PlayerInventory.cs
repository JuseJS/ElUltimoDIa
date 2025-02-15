using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<Key> keys = new List<Key>();

    public void AddKey(Key key)
    {
        if (!keys.Contains(key))
        {
            keys.Add(key);
        }
    }

    public bool HasKey(KeyType keyType)
    {
        return keys.Exists(key => key.keyType == keyType);
    }

    public void RemoveKey(KeyType keyType)
    {
        Key keyToRemove = keys.Find(key => key.keyType == keyType);
        if (keyToRemove != null)
        {
            keys.Remove(keyToRemove);
        }
    }
}