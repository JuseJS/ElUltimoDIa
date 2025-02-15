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
            //UIManager.Instance.ShowMessage($"Has obtenido: {key.keyName}");
        }
    }

    public bool HasKey(KeyType keyType)
    {
        return keys.Exists(key => key.keyType == keyType);
    }
}