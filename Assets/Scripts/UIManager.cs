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
        }

        // Ocultar todos los grupos al inicio
        if (missionGroup != null) missionGroup.alpha = 0;
        if (messageGroup != null) messageGroup.alpha = 0;
        if (interactionGroup != null) interactionGroup.alpha = 0;
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
        if (messageText == null) return;

        StopCoroutine(nameof(ShowMessageCoroutine));
        StartCoroutine(ShowMessageCoroutine(message, isError));
    }

    private IEnumerator ShowMessageCoroutine(string message, bool isError)
    {
        messageText.text = message;
        messageText.color = isError ? errorColor : successColor;
        messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, 1f);
        messageGroup.alpha = 1;

        yield return new WaitForSeconds(messageDuration);

        // Fade out
        float elapsed = 0f;
        float fadeDuration = 0.5f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            messageGroup.alpha = Mathf.Clamp01(1f - (elapsed / fadeDuration));
            yield return null;
        }
        messageGroup.alpha = 0;
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
}