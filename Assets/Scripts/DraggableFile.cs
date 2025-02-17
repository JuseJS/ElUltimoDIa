using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class DraggableFile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI fileNameText;
    [SerializeField] private string fileName;
    [SerializeField] private Transform fileGrid;
    
    public string FileName => fileName;
    
    public event Action<DraggableFile> OnDragStarted;
    public event Action<DraggableFile> OnDragEnded;
    
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        
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
    }

    public void ResetPosition()
    {
        if (fileGrid != null)
        {
            transform.SetParent(fileGrid);
            
            // Forzar actualización del GridLayoutGroup
            LayoutRebuilder.ForceRebuildLayoutImmediate(fileGrid as RectTransform);
            
            // También podemos forzar la actualización del Canvas
            Canvas.ForceUpdateCanvases();
        }
        else
        {
            Debug.LogError($"FileGrid reference is missing for {gameObject.name}");
        }
    }
}