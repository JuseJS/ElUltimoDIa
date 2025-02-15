using UnityEngine;

[CreateAssetMenu(fileName = "New Key", menuName = "Inventory/Key")]
public class Key : ScriptableObject
{
    public string keyName;
    public KeyType keyType;
    public Sprite keySprite;
}