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
            playerInventory.AddKey(keyToFind);
            UIManager.Instance.ShowMessage(successMessage);
            KeySearchManager.Instance.RegisterKeyFound(keyToFind.name);

            // Verificar si esta llave completa la misión actual
            var currentMission = MissionManager.Instance.CurrentMission;
            if (currentMission == gameMissions.findMainKeyMission && keyToFind.keyType == KeyType.MainDoor)
            {
                MissionManager.Instance.CompleteMission();
                MissionManager.Instance.StartMission(gameMissions.enterSchoolMission);
            }
            else if (currentMission == gameMissions.findClassroomKeyMission && keyToFind.keyType == KeyType.ClassroomDoor)
            {
                UIManager.Instance.ShowMessage("Has encontrado la llave. Vuelve a hablar con la secretaria.");
            }
        }
        else
        {
            UIManager.Instance.ShowMessage(failMessage, true);
        }
    }
}