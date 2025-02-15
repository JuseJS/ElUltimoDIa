using UnityEngine;
using System.Collections;

public class SecretaryNPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue Data")]
    [SerializeField] private DialogueData initialDialogue;
    [SerializeField] private DialogueData searchKeyDialogue;
    [SerializeField] private DialogueData foundKeyDialogue;
    [SerializeField] private DialogueData taskCompletedDialogue;

    private UIManager uiManager;
    private bool hasGivenMainKey = false;
    private bool hasClassroomKey = false;
    private bool hasCompletedTask = false;
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
        if (!hasGivenMainKey && playerInventory.HasKey(KeyType.MainDoor))
        {
            return initialDialogue;
        }
        else if (hasGivenMainKey && !hasClassroomKey)
        {
            return searchKeyDialogue;
        }
        else if (hasClassroomKey && !hasCompletedTask)
        {
            return foundKeyDialogue;
        }
        else if (hasCompletedTask)
        {
            return taskCompletedDialogue;
        }
        
        return null;
    }

    private IEnumerator PlayDialogue(DialogueData dialogue)
    {
        isInDialogue = true;

        foreach (var line in dialogue.lines)
        {
            uiManager.ShowDialogue(line.speaker, line.text);
            
            // Esperar input del jugador para continuar
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            yield return new WaitForSeconds(0.2f); // Pequeña pausa entre líneas
            
            // Procesar acciones especiales
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
                    hasGivenMainKey = true;
                    uiManager.ShowMessage("Has entregado la llave principal a la secretaria");
                }
                break;

            case DialogueAction.StartClassroomKeyMission:
                uiManager.UpdateMission("Encuentra la llave de la clase");
                break;

            case DialogueAction.StartGoToClassMission:
                uiManager.UpdateMission("Dirígete al aula de informática");
                uiManager.ShowMessage("La secretaria te ha dado acceso a la clase", false);
                break;

            case DialogueAction.CompleteGame:
                if (playerInventory.HasKey(KeyType.ClassroomDoor))
                {
                    playerInventory.RemoveKey(KeyType.ClassroomDoor);
                    GameManager.Instance.GameOver(true);
                }
                break;
        }
    }

    public void SetHasClassroomKey(bool value)
    {
        hasClassroomKey = value;
    }

    public void SetTaskCompleted(bool value)
    {
        hasCompletedTask = value;
    }
}