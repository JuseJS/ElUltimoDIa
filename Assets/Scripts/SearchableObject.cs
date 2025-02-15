using UnityEngine;

public class SearchableObject : MonoBehaviour, IInteractable
{
    private bool containsKey = false;
    private Key keyToFind;
    private bool hasBeenSearched = false;

    public void Initialize(bool containsKey, Key key)
    {
        this.containsKey = containsKey;
        this.keyToFind = key;
        this.hasBeenSearched = false;
    }

    public void Interact(PlayerInventory playerInventory)
    {
        if (hasBeenSearched)
        {
            UIManager.Instance.ShowMessage("Ya has buscado aquí", true);
            return;
        }

        hasBeenSearched = true;

        if (containsKey && keyToFind != null)
        {
            playerInventory.AddKey(keyToFind);
            UIManager.Instance.ShowMessage($"¡Has encontrado la {keyToFind.keyName}!");
            
            // Actualizar la misión actual si es necesario
            if (keyToFind.keyType == KeyType.MainDoor)
            {
                UIManager.Instance.UpdateMission("Dirígete a secretaría");
            }
        }
        else
        {
            UIManager.Instance.ShowMessage("No has encontrado nada", true);
        }
    }
}