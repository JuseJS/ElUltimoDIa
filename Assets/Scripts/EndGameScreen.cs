using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public class EndGameScreen : MonoBehaviour
{
    [Header("Screen Elements")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI missionsText;
    [SerializeField] private Image backgroundPanel;
    [SerializeField] private Image statsPanel;
    
    [Header("Victory Configuration")]
    [SerializeField] private Sprite victoryIcon;
    [SerializeField] private Color victoryColor;
    [SerializeField] private string victoryTitle = "¡Victoria!";
    [SerializeField] private string victoryMessage = "¡Has completado la entrega del trabajo a tiempo!";
    
    [Header("Defeat Configuration")]
    [SerializeField] private Sprite defeatIcon;
    [SerializeField] private Color defeatColor;
    [SerializeField] private string defeatTitle = "¡Tiempo Agotado!";
    [SerializeField] private string defeatMessage = "No has logrado entregar el trabajo a tiempo.";
    
    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.5f;
    
    [Header("Buttons")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
        
        // Configurar TextMeshPro para mantener el layout
        ConfigureTextComponents();
    }

    private void ConfigureTextComponents()
    {
        // Configurar TextMeshPro para mantener tamaño y posición
        TextMeshProUGUI[] texts = { titleText, messageText, timeText, missionsText };
        foreach (var text in texts)
        {
            if (text != null)
            {
                text.enableAutoSizing = false;  // Deshabilitar auto-sizing
                text.overflowMode = TextOverflowModes.Truncate;  // Evitar desbordamiento
                text.enableWordWrapping = true;  // Permitir wrapping para textos largos
            }
        }
    }

    public void Show(bool isVictory, string timeLeft, string missionsCompleted)
    {
        // Habilitar el cursor al mostrar la pantalla
        EnableCursor();
        
        // Configurar la pantalla según el estado
        ConfigureScreen(isVictory, timeLeft, missionsCompleted);
        
        // Mostrar y animar
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    private void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void DisableCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ConfigureScreen(bool isVictory, string timeLeft, string missionsCompleted)
    {
        // Configurar icono y colores
        iconImage.sprite = isVictory ? victoryIcon : defeatIcon;
        backgroundPanel.color = isVictory ? victoryColor : defeatColor;
        
        // Configurar textos con verificación de nulos
        if (titleText != null) titleText.text = isVictory ? victoryTitle : defeatTitle;
        if (messageText != null) messageText.text = isVictory ? victoryMessage : defeatMessage;
        if (timeText != null) timeText.text = timeLeft;
        if (missionsText != null) missionsText.text = missionsCompleted;
        
        // Actualizar color del panel de estadísticas
        if (statsPanel != null)
        {
            Color statsPanelColor = isVictory ? victoryColor : defeatColor;
            statsPanelColor.a = 0.1f;
            statsPanel.color = statsPanelColor;
        }
    }

    public void Hide()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
        
        // Deshabilitar el cursor al ocultar la pantalla
        DisableCursor();
    }

    public void SetupButtons(UnityAction retryAction, UnityAction menuAction)
    {
        if (retryButton != null)
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(retryAction);
        }
        
        if (menuButton != null)
        {
            menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(menuAction);
        }
    }

    private void OnDisable()
    {
        // Asegurar que el cursor se desactive al cerrar la pantalla
        DisableCursor();
    }
}