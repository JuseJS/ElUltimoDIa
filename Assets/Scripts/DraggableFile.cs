using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class DraggableFile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI fileNameText;
    [SerializeField] private string fileName;
    
    public string FileName => fileName;
    
    public event Action<DraggableFile> OnDragStarted;
    public event Action<DraggableFile> OnDragEnded;
    
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = rectTransform.anchoredPosition;
        
        if (fileNameText != null)
        {
            fileNameText.text = fileName;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        OnDragStarted?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        OnDragEnded?.Invoke(this);
        
        // Si no se soltó en la zona de drop, volver a la posición original
        if (!eventData.pointerEnter || !eventData.pointerEnter.GetComponent<DropZone>())
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = originalPosition;
    }
}