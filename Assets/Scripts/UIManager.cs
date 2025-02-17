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
    [SerializeField] private float messageDuration = 1.5f;
    [SerializeField] private float fadeInDuration = 0.2f;
    [SerializeField] private float fadeOutDuration = 0.5f;

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

    private void Start()
    {
        MissionManager.Instance.OnMissionStarted += HandleMissionStarted;
        MissionManager.Instance.OnMissionCompleted += HandleMissionCompleted;
    }

    private void OnDestroy()
    {
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.OnMissionStarted -= HandleMissionStarted;
            MissionManager.Instance.OnMissionCompleted -= HandleMissionCompleted;
        }
    }

    private void HandleMissionStarted(Mission mission)
    {
        UpdateMission(mission.description);
    }

    private void HandleMissionCompleted(Mission mission)
    {
        ShowMessage("Misión completada", false);
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

        if (currentMessageCoroutine != null)
        {
            StopCoroutine(currentMessageCoroutine);
        }

        currentMessageCoroutine = StartCoroutine(ShowMessageCoroutine(message, isError));
    }

    private IEnumerator ShowMessageCoroutine(string message, bool isError)
    {
        messageText.text = message;
        messageText.color = isError ? errorColor : successColor;

        float elapsedTime = 0;
        messageGroup.alpha = 0;
        
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            messageGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeInDuration);
            yield return null;
        }
        messageGroup.alpha = 1;

        yield return new WaitForSeconds(messageDuration);

        // Fade out
        elapsedTime = 0;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            messageGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeOutDuration);
            yield return null;
        }
        messageGroup.alpha = 0;

        currentMessageCoroutine = null;
    }

    public void ShowInteractionText(string text)
    {
        if (interactionText != null && interactionGroup != null)
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
            if (speakerText != null) speakerText.text = speaker;
            if (dialogueText != null) dialogueText.text = text;
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