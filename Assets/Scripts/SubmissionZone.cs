using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class SubmissionZone : MonoBehaviour, IDropHandler
{
    public event Action<string> OnFileSubmitted;
    
    [SerializeField] private RectTransform submissionPreview;
    [SerializeField] private Button submitButton;
    
    private DraggableFile currentFile;

    private void Start()
    {
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(HandleSubmission);
            submitButton.interactable = false;
        }
        
        if (submissionPreview != null)
        {
            submissionPreview.gameObject.SetActive(false);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableFile droppedFile = eventData.pointerDrag.GetComponent<DraggableFile>();
        if (droppedFile != null)
        {
            currentFile = droppedFile;
            
            // Actualizar vista previa
            if (submissionPreview != null)
            {
                submissionPreview.gameObject.SetActive(true);
                // Aquí podrías actualizar el nombre del archivo en la vista previa
            }
            
            if (submitButton != null)
            {
                submitButton.interactable = true;
            }
        }
    }

    private void HandleSubmission()
    {
        if (currentFile != null)
        {
            OnFileSubmitted?.Invoke(currentFile.FileName);
            
            // Limpiar la zona de envío
            currentFile = null;
            if (submissionPreview != null)
            {
                submissionPreview.gameObject.SetActive(false);
            }
            submitButton.interactable = false;
        }
    }

    private void OnDestroy()
    {
        if (submitButton != null)
        {
            submitButton.onClick.RemoveListener(HandleSubmission);
        }
    }
}