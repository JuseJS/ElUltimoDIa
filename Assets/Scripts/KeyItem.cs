using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    public Key keyData;
    
    public void Interact(PlayerInventory playerInventory)
    {
        playerInventory.AddKey(keyData);
        gameObject.SetActive(false);
    }
}