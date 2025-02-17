using UnityEngine;

public class SearchableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private MissionData gameMissions;
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
        if (hasBeenSearched)
        {
            UIManager.Instance.ShowMessage("Ya has buscado aquí", true);
            return;
        }

        hasBeenSearched = true;

        if (containsKey && keyToFind != null)
        {
            // Si es la llave del aula y la misión actual no es findClassroomKeyMission
            if (keyToFind.keyType == KeyType.ClassroomDoor && 
                MissionManager.Instance.CurrentMission != gameMissions.findClassroomKeyMission)
            {
                UIManager.Instance.ShowMessage("Has encontrado una llave, pero deberías hablar primero con la secretaria", true);
                hasBeenSearched = false;
                return;
            }

            playerInventory.AddKey(keyToFind);
            UIManager.Instance.ShowMessage(successMessage);
            KeySearchManager.Instance.RegisterKeyFound(keyToFind.name);

            // Solo completar la misión si es la llave principal
            if (MissionManager.Instance.CurrentMission == gameMissions.findMainKeyMission && 
                keyToFind.keyType == KeyType.MainDoor)
            {
                MissionManager.Instance.CompleteMission();
            }
            // Para la llave del aula, solo mostrar un mensaje
            else if (keyToFind.keyType == KeyType.ClassroomDoor && 
                     MissionManager.Instance.CurrentMission == gameMissions.findClassroomKeyMission)
            {
                UIManager.Instance.ShowMessage("¡Has encontrado la llave del aula! Vuelve con la secretaria.");
            }
        }
        else
        {
            UIManager.Instance.ShowMessage(failMessage, true);
        }
    }
}