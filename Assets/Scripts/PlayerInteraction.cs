using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    private PlayerInventory inventory;
    private IInteractable currentInteractable;
    private Transform currentInteractableTransform;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;

        CheckForInteractables();
        HandleInteractionInput();
    }

    private void CheckForInteractables()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactionLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            
            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    // Nuevo objeto interactuable encontrado
                    if (currentInteractable != null)
                    {
                        UIManager.Instance.HideInteractionText(currentInteractableTransform.position);
                    }
                    
                    currentInteractable = interactable;
                    currentInteractableTransform = hit.transform;
                    
                    string interactionText = (interactable as MonoBehaviour)?.GetComponent<InteractionPrompt>()?.promptText 
                        ?? "Pulsa E para interactuar";
                    UIManager.Instance.ShowInteractionText(hit.point, interactionText);
                }
            }
        }
        else if (currentInteractable != null)
        {
            // Ya no hay objeto interactuable en rango
            UIManager.Instance.HideInteractionText(currentInteractableTransform.position);
            currentInteractable = null;
            currentInteractableTransform = null;
        }
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactionKey) && currentInteractable != null)
        {
            currentInteractable.Interact(inventory);
        }
    }
}