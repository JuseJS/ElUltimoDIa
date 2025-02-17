using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Mission UI")]
    [SerializeField] private TextMeshProUGUI currentMissionText;
    [SerializeField] private CanvasGroup missionGroup;

    [Header("Message UI")]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup messageGroup;
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color errorColor = Color.red;
    [SerializeField] private float messageDuration = 3f;

    [Header("Interaction UI")]
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private CanvasGroup interactionGroup;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Coroutine currentMessageCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Ocultar todos los grupos al inicio
        if (missionGroup != null) missionGroup.alpha = 1;
        if (messageGroup != null) messageGroup.alpha = 0;
        if (interactionGroup != null) interactionGroup.alpha = 0;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    public void UpdateMission(string missionText)
    {
        if (currentMissionText != null)
        {
            currentMissionText.text = missionText;
            if (missionGroup != null) missionGroup.alpha = 1;
        }
    }

    public void ShowMessage(string message, bool isError = false)
    {
        if (messageText == null || messageGroup == null) return;

        // Si hay una corrutina en curso, la detenemos
        if (currentMessageCoroutine != null)
        {
            StopCoroutine(currentMessageCoroutine);
            currentMessageCoroutine = null;
        }

        // Iniciamos la nueva corrutina y guardamos su referencia
        currentMessageCoroutine = StartCoroutine(ShowMessageCoroutine(message, isError));
    }

    private IEnumerator ShowMessageCoroutine(string message, bool isError)
    {
        // Asegurar que el mensaje sea visible inmediatamente
        messageGroup.alpha = 1;
        messageText.text = message;
        messageText.color = isError ? errorColor : successColor;

        // Esperar la duración del mensaje
        yield return new WaitForSeconds(messageDuration);

        // Fade out
        float elapsed = 0f;
        float fadeDuration = 0.5f;
        
        Color startColor = messageText.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / fadeDuration;
            
            messageGroup.alpha = Mathf.Lerp(1f, 0f, normalizedTime);
            messageText.color = new Color(
                startColor.r,
                startColor.g,
                startColor.b,
                Mathf.Lerp(1f, 0f, normalizedTime)
            );
            
            yield return null;
        }

        // Asegurar que el mensaje esté completamente oculto
        messageGroup.alpha = 0;
        messageText.color = new Color(startColor.r, startColor.g, startColor.b, 0);
        
        // Limpiar la referencia de la corrutina
        currentMessageCoroutine = null;
    }

    public void ShowInteractionText(string text)
    {
        if (interactionText != null)
        {
            interactionText.text = text;
            interactionGroup.alpha = 1;
        }
    }

    public void HideInteractionText()
    {
        if (interactionGroup != null)
        {
            interactionGroup.alpha = 0;
        }
    }

    public void ShowDialogue(string speaker, string text)
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
            speakerText.text = speaker;
            dialogueText.text = text;
        }
    }

    public void HideDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }
}