using CharacterScript;
using UnityEngine;
using UnityEngine.UI;

public class Computer : MonoBehaviour, IInteractable
{
    [Header("Referencias")]
    [SerializeField] private GameObject computerPanel;
    [SerializeField] private MissionData gameMissions;
    [SerializeField] private Button shutdownButton;
    [SerializeField] private DraggableFile[] draggableFiles;
    [SerializeField] private SubmissionZone submissionZone;
    [SerializeField] private string correctFileName = "Trabajo_Final.doc";
    
    private bool isOn = false;
    private FPSController playerController;

    private void Start()
    {
        computerPanel.SetActive(false);
        shutdownButton.onClick.AddListener(ShutdownComputer);
        
        // Configurar archivos
        if (draggableFiles != null && draggableFiles.Length > 0)
        {
            foreach (var file in draggableFiles)
            {
                file.Initialize();
            }
        }

        if (submissionZone != null)
        {
            submissionZone.OnFileSubmitted += HandleFileSubmission;
        }
    }

    private void OnDestroy()
    {
        if (submissionZone != null)
        {
            submissionZone.OnFileSubmitted -= HandleFileSubmission;
        }
    }

    public void Interact(PlayerInventory playerInventory)
    {
        if (MissionManager.Instance.CurrentMission == gameMissions.accessComputerMission)
        {
            MissionManager.Instance.CompleteMission();
        }

        TurnOnComputer();
    }

    private void TurnOnComputer()
    {
        isOn = true;
        computerPanel.SetActive(true);
        
        // Mostrar y desbloquear el cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // Desactivar el controlador FPS
        if (playerController == null)
        {
            playerController = FindObjectOfType<FPSController>();
        }
        
        if (playerController != null)
        {
            playerController.canMove = false;
        }
    }

    private void ShutdownComputer()
    {
        isOn = false;
        computerPanel.SetActive(false);
        
        // Ocultar y bloquear el cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        // Reactivar el controlador FPS
        if (playerController != null)
        {
            playerController.canMove = true;
        }
    }

    private void HandleFileSubmission(string fileName)
    {
        if (fileName == correctFileName)
        {
            if (MissionManager.Instance.CurrentMission == gameMissions.submitWorkMission)
            {
                UIManager.Instance.ShowMessage("¡Trabajo enviado correctamente!");
                MissionManager.Instance.CompleteMission();
                ShutdownComputer();
            }
        }
        else
        {
            UIManager.Instance.ShowMessage("Has enviado el archivo incorrecto.", true);
        }
    }
}