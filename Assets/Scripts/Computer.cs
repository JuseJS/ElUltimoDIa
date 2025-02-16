using CharacterScript;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Computer : MonoBehaviour, IInteractable
{
    [Header("UI References")]
    [SerializeField] private GameObject computerScreen;
    [SerializeField] private Button shutdownButton;
    [SerializeField] private Button submitButton;
    [SerializeField] private GameObject dropZone;
    
    [Header("Camera Control")]
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    
    [Header("Files")]
    [SerializeField] private DraggableFile[] draggableFiles;
    [SerializeField] private string correctFileName = "Trabajo_Final.doc";
    
    [Header("Mission Data")]
    [SerializeField] private MissionData gameMissions;
    
    private FPSController playerController;
    private DraggableFile currentDraggedFile;
    private DraggableFile droppedFile;
    private float originalCameraSpeed;

    private void Start()
    {
        Debug.Log("Computer: Iniciando configuración");
        
        computerScreen.SetActive(false);
        
        shutdownButton.onClick.AddListener(ShutdownComputer);
        submitButton.onClick.AddListener(HandleSubmission);
        submitButton.interactable = false;
        
        foreach (var file in draggableFiles)
        {
            if (file != null)
            {
                file.OnDragStarted += HandleFileDragStarted;
                file.OnDragEnded += HandleFileDragEnded;
            }
        }
        
        var dropZoneComponent = dropZone.GetComponent<DropZone>();
        if (dropZoneComponent != null)
        {
            dropZoneComponent.OnFileDropped += HandleFileDropped;
        }
        
        if (freeLookCamera == null)
        {
            freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
        }
        
        if (freeLookCamera != null)
        {
            originalCameraSpeed = freeLookCamera.m_XAxis.m_MaxSpeed;
        }
    }

    private void OnDestroy()
    {
        if (shutdownButton != null) shutdownButton.onClick.RemoveListener(ShutdownComputer);
        if (submitButton != null) submitButton.onClick.RemoveListener(HandleSubmission);
        
        foreach (var file in draggableFiles)
        {
            if (file != null)
            {
                file.OnDragStarted -= HandleFileDragStarted;
                file.OnDragEnded -= HandleFileDragEnded;
            }
        }
        
        var dropZoneComponent = dropZone?.GetComponent<DropZone>();
        if (dropZoneComponent != null)
        {
            dropZoneComponent.OnFileDropped -= HandleFileDropped;
        }
    }

    public void Interact(PlayerInventory playerInventory)
    {
        Debug.Log("Computer: Interacción iniciada");
        
        if (MissionManager.Instance != null && gameMissions != null && 
            MissionManager.Instance.CurrentMission == gameMissions.accessComputerMission)
        {
            Debug.Log("Computer: Completando misión de acceso");
            MissionManager.Instance.CompleteMission();
        }
        
        TurnOnComputer();
    }

    private void TurnOnComputer()
    {
        computerScreen.SetActive(true);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        if (playerController == null)
        {
            playerController = FindObjectOfType<FPSController>();
        }
        
        if (playerController != null)
        {
            playerController.canMove = false;
        }
        
        if (freeLookCamera != null)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 0f;
            freeLookCamera.m_YAxis.m_MaxSpeed = 0f;
        }
    }

    private void ShutdownComputer()
    {
        Debug.Log("Computer: Apagando computadora");
        computerScreen.SetActive(false);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        if (playerController != null)
        {
            playerController.canMove = true;
        }
        
        if (freeLookCamera != null)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = originalCameraSpeed;
            freeLookCamera.m_YAxis.m_MaxSpeed = 0;
        }
        
        // Limpiar estado
        currentDraggedFile = null;
        droppedFile = null;
        submitButton.interactable = false;
    }

    private void HandleFileDragStarted(DraggableFile file)
    {
        Debug.Log($"Computer: Iniciando arrastre de archivo: {file.FileName}");
        currentDraggedFile = file;
    }

    private void HandleFileDragEnded(DraggableFile file)
    {
        Debug.Log($"Computer: Finalizando arrastre de archivo: {file.FileName}");
    }

    private void HandleFileDropped(DraggableFile file)
    {
        Debug.Log($"Computer: Archivo soltado en drop zone: {file.FileName}");
        droppedFile = file;
        submitButton.interactable = true;
    }

    private void HandleSubmission()
    {
        Debug.Log("Computer: Iniciando proceso de envío");
        
        // Usamos droppedFile en lugar de currentDraggedFile
        if (droppedFile == null)
        {
            Debug.LogError("Computer: No hay archivo para enviar");
            return;
        }

        Debug.Log($"Computer: Archivo actual: {droppedFile.FileName}, Archivo esperado: {correctFileName}");
        
        if (droppedFile.FileName == correctFileName)
        {
            Debug.Log("Computer: Nombre de archivo correcto");
            
            if (MissionManager.Instance != null && 
                MissionManager.Instance.CurrentMission == gameMissions.submitWorkMission)
            {
                Debug.Log("Computer: Misión correcta, completando envío");
                UIManager.Instance.ShowMessage("¡Trabajo enviado correctamente!");
                MissionManager.Instance.CompleteMission();
                ShutdownComputer();
            }
            else
            {
                Debug.LogWarning("Computer: Misión incorrecta o MissionManager.Instance es null");
                UIManager.Instance.ShowMessage("No es el momento de enviar el trabajo.", true);
            }
        }
        else
        {
            Debug.Log("Computer: Archivo incorrecto");
            UIManager.Instance.ShowMessage("Has enviado el archivo incorrecto.", true);
        }
        
        droppedFile.ResetPosition();
        droppedFile = null;
        submitButton.interactable = false;
    }
}