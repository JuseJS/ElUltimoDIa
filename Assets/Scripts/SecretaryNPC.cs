using UnityEngine;
using System.Collections;

public class SecretaryNPC : MonoBehaviour, IInteractable
{
    [Header("Referencias")]
    [SerializeField] private MissionData gameMissions;
    
    [Header("Dialogue Data")]
    [SerializeField] private DialogueData initialDialogue;
    [SerializeField] private DialogueData searchKeyDialogue;
    [SerializeField] private DialogueData foundKeyDialogue;
    [SerializeField] private DialogueData taskCompletedDialogue;

    private UIManager uiManager;
    private bool isInDialogue = false;
    private PlayerInventory playerInventory;

    private void Start()
    {
        uiManager = UIManager.Instance;
    }

    public void Interact(PlayerInventory inventory)
    {
        if (isInDialogue) return;

        playerInventory = inventory;
        DialogueData currentDialogue = DetermineDialogue();

        if (currentDialogue != null)
        {
            StartCoroutine(PlayDialogue(currentDialogue));
        }
    }

    private DialogueData DetermineDialogue()
    {
        Mission currentMission = MissionManager.Instance.CurrentMission;

        if (currentMission == gameMissions.talkToSecretaryMission && playerInventory.HasKey(KeyType.MainDoor))
        {
            return initialDialogue;
        }
        else if (currentMission == gameMissions.findClassroomKeyMission)
        {
            if (playerInventory.HasKey(KeyType.ClassroomDoor))
            {
                return foundKeyDialogue;
            }
            return searchKeyDialogue;
        }
        else if (currentMission == gameMissions.submitWorkMission || 
                 currentMission == gameMissions.returnKeykMission)
        {
            return taskCompletedDialogue;
        }

        return null;
    }

    private IEnumerator PlayDialogue(DialogueData dialogue)
    {
        isInDialogue = true;
        Input.ResetInputAxes();

        foreach (var line in dialogue.lines)
        {
            uiManager.ShowDialogue(line.speaker, line.text);

            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            yield return new WaitForSeconds(0.2f);

            ProcessDialogueAction(line.action);
        }

        uiManager.HideDialogue();
        isInDialogue = false;
    }

    private void ProcessDialogueAction(DialogueAction action)
    {
        switch (action)
        {
            case DialogueAction.RemoveMainKey:
                if (playerInventory.HasKey(KeyType.MainDoor))
                {
                    playerInventory.RemoveKey(KeyType.MainDoor);
                    MissionManager.Instance.CompleteMission();
                    uiManager.ShowMessage("Has entregado la llave principal a la secretaria");
                }
                break;

            case DialogueAction.StartClassroomKeyMission:
                uiManager.ShowMessage("Busca la llave del aula de informática");
                break;

            case DialogueAction.StartGoToClassMission:
                if (MissionManager.Instance.CurrentMission == gameMissions.findClassroomKeyMission &&
                    playerInventory.HasKey(KeyType.ClassroomDoor))
                {
                    MissionManager.Instance.CompleteMission();
                    uiManager.ShowMessage("Puedes acceder al aula de informática");
                }
                break;

            case DialogueAction.CompleteGame:
                if (MissionManager.Instance.CurrentMission == gameMissions.returnKeykMission)
                {
                    playerInventory.RemoveKey(KeyType.ClassroomDoor);
                    MissionManager.Instance.CompleteMission();
                    GameManager.Instance.GameOver(true);
                }
                break;
        }
    }
}