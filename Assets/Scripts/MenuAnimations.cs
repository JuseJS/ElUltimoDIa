using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class MainMenuAnimations : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float buttonHoverScale = 1.05f;
    [SerializeField] private float buttonHoverDuration = 0.2f;
    
    [Header("UI References")]
    [SerializeField] private CanvasGroup mainCanvasGroup;
    [SerializeField] private RectTransform titleRect;
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private RectTransform controlsPanel;
    
    private void Start()
    {
        InitializeAnimations();
    }
    
    private void InitializeAnimations()
    {
        // Configurar estado inicial
        mainCanvasGroup.alpha = 0;
        
        if (titleRect != null)
            titleRect.localScale = Vector3.zero;
            
        if (controlsPanel != null)
            controlsPanel.localScale = Vector3.zero;
        
        // Animación de entrada
        DOTween.Sequence()
            .AppendCallback(() => {
                // Fade in del canvas
                mainCanvasGroup.DOFade(1, fadeInDuration);
                
                // Animación del título
                if (titleRect != null)
                    titleRect.DOScale(1, fadeInDuration).SetEase(Ease.OutBack);
                
                // Animación de los botones
                if (buttons != null)
                {
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i].DOScale(1, fadeInDuration)
                            .SetDelay(fadeInDuration + (i * 0.1f))
                            .SetEase(Ease.OutBack);
                    }
                }
                
                // Animación del panel de controles
                if (controlsPanel != null)
                    controlsPanel.DOScale(1, fadeInDuration)
                        .SetDelay(fadeInDuration + 0.3f)
                        .SetEase(Ease.OutBack);
            });
        
        // Configurar animaciones de hover para los botones
        foreach (var button in buttons)
        {
            var btn = button.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => OnButtonClick(button));
                
                // Eventos de hover
                var eventTrigger = button.gameObject.AddComponent<EventTrigger>();
                
                var pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
                pointerEnter.callback.AddListener((data) => OnButtonHoverEnter(button));
                eventTrigger.triggers.Add(pointerEnter);
                
                var pointerExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
                pointerExit.callback.AddListener((data) => OnButtonHoverExit(button));
                eventTrigger.triggers.Add(pointerExit);
            }
        }
    }
    
    private void OnButtonHoverEnter(RectTransform button)
    {
        button.DOScale(buttonHoverScale, buttonHoverDuration)
            .SetEase(Ease.OutQuad);
    }
    
    private void OnButtonHoverExit(RectTransform button)
    {
        button.DOScale(1f, buttonHoverDuration)
            .SetEase(Ease.OutQuad);
    }
    
    private void OnButtonClick(RectTransform button)
    {
        button.DOPunchScale(Vector3.one * 0.1f, 0.2f, 1, 0.5f);
    }
    
    private void OnDestroy()
    {
        DOTween.Kill(titleRect);
        DOTween.Kill(controlsPanel);
        foreach (var button in buttons)
        {
            DOTween.Kill(button);
        }
    }
}