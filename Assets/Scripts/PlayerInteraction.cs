using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerInventory))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private Transform interactionCenter; // Punto desde donde se detecta la interacción

    private PlayerInventory inventory;
    private IInteractable currentInteractable;
    private Transform currentInteractableTransform;
    private List<IInteractable> nearbyInteractables = new List<IInteractable>();

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();

        // Si no se asignó un centro de interacción, usar el transform del jugador
        if (interactionCenter == null)
        {
            interactionCenter = transform;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;

        CheckForInteractables();
        HandleInteractionInput();
    }

    private void CheckForInteractables()
    {
        // Obtener todos los colliders en el radio de interacción
        Collider[] hitColliders = Physics.OverlapSphere(interactionCenter.position, interactionRadius);

        // Limpiar la lista anterior de interactables
        nearbyInteractables.Clear();

        foreach (var hitCollider in hitColliders)
        {
            IInteractable interactable = hitCollider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                nearbyInteractables.Add(interactable);
            }
        }

        // Si no hay interactables cerca, limpiar el actual
        if (nearbyInteractables.Count == 0)
        {
            if (currentInteractable != null)
            {
                UIManager.Instance.HideInteractionText(); // ✅ Corrección clave (sin parámetros)
                currentInteractable = null;
                currentInteractableTransform = null;
            }
            return;
        }

        // Encontrar el interactable más cercano
        IInteractable closestInteractable = null;
        Transform closestTransform = null;
        float closestDistance = float.MaxValue;

        foreach (var interactable in nearbyInteractables)
        {
            MonoBehaviour mb = interactable as MonoBehaviour;
            if (mb != null)
            {
                float distance = Vector3.Distance(interactionCenter.position, mb.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                    closestTransform = mb.transform;
                }
            }
        }

        // Si encontramos un nuevo interactable más cercano
        if (closestInteractable != currentInteractable)
        {
            if (currentInteractable != null)
            {
                UIManager.Instance.HideInteractionText();
            }

            currentInteractable = closestInteractable;
            currentInteractableTransform = closestTransform;

            if (currentInteractable != null)
            {
                MonoBehaviour mb = currentInteractable as MonoBehaviour;
                InteractionPrompt prompt = mb.GetComponent<InteractionPrompt>();
                string interactionText = prompt != null ? prompt.promptText : "Pulsa E para interactuar";

                UIManager.Instance.ShowInteractionText(interactionText);
            }
        }
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactionKey) && currentInteractable != null)
        {
            Debug.Log($"Interactuando con {(currentInteractable as MonoBehaviour)?.gameObject.name}");
            currentInteractable.Interact(inventory);
        }
    }

    // Visualización en el editor para debug
    private void OnDrawGizmos()
    {
        if (interactionCenter != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionCenter.position, interactionRadius);
        }
    }
}