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

    private bool isInitialized = false;
    
    private void Awake()
    {
        // No inicializamos aquí, esperamos a que MenuManager lo haga
        ValidateReferences();
    }

    private void ValidateReferences()
    {
        if (mainCanvasGroup == null)
        {
            Debug.LogError("MainMenuAnimations: Missing mainCanvasGroup reference!");
            enabled = false;
            return;
        }

        // Verificar y logear advertencias para referencias faltantes
        if (titleRect == null) Debug.LogWarning("MainMenuAnimations: Missing titleRect reference");
        if (buttons == null || buttons.Length == 0) Debug.LogWarning("MainMenuAnimations: No buttons assigned");
        if (controlsPanel == null) Debug.LogWarning("MainMenuAnimations: Missing controlsPanel reference");
    }
    
    public void InitializeAnimations()
    {
        if (isInitialized) return;
        
        // Configurar estado inicial
        if (mainCanvasGroup != null)
        {
            mainCanvasGroup.alpha = 0;
            mainCanvasGroup.interactable = false; // Importante: deshabilitar interacción hasta que termine la animación
        }
        
        // Resetear escalas solo si las referencias existen
        if (titleRect != null)
            titleRect.localScale = Vector3.zero;
            
        if (controlsPanel != null)
            controlsPanel.localScale = Vector3.zero;

        if (buttons != null)
        {
            foreach (var button in buttons)
            {
                if (button != null)
                    button.localScale = Vector3.zero;
            }
        }
        
        // Iniciar secuencia de animación
        DOTween.Sequence()
            .AppendCallback(() => {
                // Fade in del canvas
                if (mainCanvasGroup != null)
                {
                    mainCanvasGroup.DOFade(1, fadeInDuration)
                        .OnComplete(() => mainCanvasGroup.interactable = true);
                }
                
                // Animación del título
                if (titleRect != null)
                {
                    titleRect.DOScale(1, fadeInDuration)
                        .SetEase(Ease.OutBack);
                }
                
                // Animación de los botones
                if (buttons != null)
                {
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        if (buttons[i] != null)
                        {
                            buttons[i].DOScale(1, fadeInDuration)
                                .SetDelay(fadeInDuration + (i * 0.1f))
                                .SetEase(Ease.OutBack);
                        }
                    }
                }
                
                // Animación del panel de controles
                if (controlsPanel != null)
                {
                    controlsPanel.DOScale(1, fadeInDuration)
                        .SetDelay(fadeInDuration + 0.3f)
                        .SetEase(Ease.OutBack);
                }
            });
        
        SetupButtonInteractions();
        isInitialized = true;
    }

    private void SetupButtonInteractions()
    {
        if (buttons == null) return;

        foreach (var button in buttons)
        {
            if (button == null) continue;

            var btn = button.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => OnButtonClick(button));
                
                var eventTrigger = button.gameObject.GetComponent<EventTrigger>();
                if (eventTrigger == null)
                    eventTrigger = button.gameObject.AddComponent<EventTrigger>();
                
                eventTrigger.triggers.Clear();
                
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
        if (button != null)
        {
            button.DOScale(buttonHoverScale, buttonHoverDuration)
                .SetEase(Ease.OutQuad);
        }
    }
    
    private void OnButtonHoverExit(RectTransform button)
    {
        if (button != null)
        {
            button.DOScale(1f, buttonHoverDuration)
                .SetEase(Ease.OutQuad);
        }
    }
    
    private void OnButtonClick(RectTransform button)
    {
        if (button != null)
        {
            button.DOPunchScale(Vector3.one * 0.1f, 0.2f, 1, 0.5f);
        }
    }
    
    private void OnDestroy()
    {
        // Matar todas las animaciones DOTween asociadas a nuestros objetos
        if (titleRect != null) DOTween.Kill(titleRect);
        if (controlsPanel != null) DOTween.Kill(controlsPanel);
        if (buttons != null)
        {
            foreach (var button in buttons)
            {
                if (button != null) DOTween.Kill(button);
            }
        }
    }

    private void OnDisable()
    {
        // Asegurar que las animaciones se detengan al deshabilitar el componente
        OnDestroy();
    }
}