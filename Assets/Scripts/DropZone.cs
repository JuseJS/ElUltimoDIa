using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class DropZone : MonoBehaviour, IDropHandler
{
    public event Action<DraggableFile> OnFileDropped;
    [SerializeField] private TextMeshProUGUI dropZoneText;
    private string defaultText = "Arrastra aquí tu archivo";
    
    private void Start()
    {
        if (dropZoneText != null)
        {
            dropZoneText.text = defaultText;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableFile droppedFile = eventData.pointerDrag.GetComponent<DraggableFile>();
        if (droppedFile != null)
        {
            OnFileDropped?.Invoke(droppedFile);
            
            if (dropZoneText != null)
            {
                dropZoneText.text = droppedFile.FileName;
            }
        }
    }

    public void ResetDropZone()
    {
        if (dropZoneText != null)
        {
            dropZoneText.text = defaultText;
        }
    }
}