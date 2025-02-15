using UnityEngine;

public class SearchableObject : MonoBehaviour, IInteractable
{
    private bool containsKey = false;
    private Key keyToFind;
    private bool hasBeenSearched = false;
    private string failMessage;
    private string successMessage;

    public void Initialize(bool containsKey, Key key, string failMessage, string successMessage)
    {
        this.containsKey = containsKey;
        this.keyToFind = key;
        this.hasBeenSearched = false;
        this.failMessage = failMessage;
        this.successMessage = successMessage;
    }

    public void Interact(PlayerInventory playerInventory)
    {
        Debug.Log($"Interactuando con {gameObject.name}");

        if (hasBeenSearched)
        {
            Debug.Log($"Objeto {gameObject.name} ya fue buscado anteriormente");
            UIManager.Instance.ShowMessage("Ya has buscado aquí", true);
            return;
        }

        hasBeenSearched = true;

        if (containsKey && keyToFind != null)
        {
            Debug.Log($"¡Llave encontrada en {gameObject.name}! Tipo: {keyToFind.keyType}");
            playerInventory.AddKey(keyToFind);
            UIManager.Instance.ShowMessage(successMessage);
            KeySearchManager.Instance.RegisterKeyFound(keyToFind.name);
            UpdateMissionBasedOnKey();
        }
        else
        {
            Debug.Log($"No se encontró llave en {gameObject.name}. containsKey: {containsKey}, keyToFind: {keyToFind}");
            UIManager.Instance.ShowMessage(failMessage, true);
        }
    }

    private void UpdateMissionBasedOnKey()
    {
        if (keyToFind.keyType == KeyType.MainDoor)
        {
            UIManager.Instance.UpdateMission("Dirígete a secretaría");
        }
        else if (keyToFind.keyType == KeyType.ClassroomDoor)
        {
            UIManager.Instance.UpdateMission("Vuelve a hablar con la secretaria");
        }
    }
}