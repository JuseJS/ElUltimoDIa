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
    [SerializeField] private GameObject interactionTextPrefab;
    
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
    }

    public void UpdateMission(string missionText)
    {
        currentMissionText.text = missionText;
        // Opcional: Añadir una animación o efecto cuando la misión cambia
        StartCoroutine(AnimateMissionUpdate());
    }

    private IEnumerator AnimateMissionUpdate()
    {
        // Fade out
        float duration = 0.5f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            missionGroup.alpha = 1f - (elapsed / duration);
            yield return null;
        }

        // Fade in
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            missionGroup.alpha = elapsed / duration;
            yield return null;
        }

        missionGroup.alpha = 1f;
    }

    public void ShowMessage(string message, bool isError = false)
    {
        StopCoroutine(nameof(ShowMessageCoroutine));
        StartCoroutine(ShowMessageCoroutine(message, isError));
    }

    private IEnumerator ShowMessageCoroutine(string message, bool isError)
    {
        messageText.text = message;
        messageText.color = isError ? errorColor : successColor;
        messageGroup.alpha = 1f;

        yield return new WaitForSeconds(messageDuration);

        // Fade out
        float elapsed = 0f;
        float fadeDuration = 0.5f;
        
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            messageGroup.alpha = 1f - (elapsed / fadeDuration);
            yield return null;
        }

        messageGroup.alpha = 0f;
    }

    public void ShowInteractionText(Vector3 worldPosition, string text)
    {
        // Convertir la posición del mundo a posición de pantalla
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        
        // Crear el texto de interacción si no existe
        GameObject interactionText = Instantiate(interactionTextPrefab, transform);
        interactionText.transform.position = screenPos;
        
        TextMeshProUGUI tmpText = interactionText.GetComponent<TextMeshProUGUI>();
        tmpText.text = text;

        // Añadir un componente para seguir la posición del objeto
        InteractionTextFollower follower = interactionText.AddComponent<InteractionTextFollower>();
        follower.Initialize(worldPosition);
    }

    public void HideInteractionText(Vector3 worldPosition)
    {
        // Buscar y destruir el texto de interacción asociado a esta posición
        InteractionTextFollower[] followers = FindObjectsOfType<InteractionTextFollower>();
        foreach (var follower in followers)
        {
            if (Vector3.Distance(follower.TargetPosition, worldPosition) < 0.1f)
            {
                Destroy(follower.gameObject);
            }
        }
    }
}
