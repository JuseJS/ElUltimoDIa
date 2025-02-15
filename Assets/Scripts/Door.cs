using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public KeyType requiredKeyType;
    public bool isLocked = true;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact(PlayerInventory playerInventory)
    {
        if (!isLocked)
        {
            // La puerta ya está desbloqueada, solo ábrela
            ToggleDoor();
            return;
        }

        if (playerInventory.HasKey(requiredKeyType))
        {
            isLocked = false;
            ToggleDoor();
        }
        else
        {
            // Mostrar mensaje de que necesitas la llave correcta
            UIManager.Instance.ShowMessage("Necesitas la llave correcta para abrir esta puerta.");
        }
    }

    private void ToggleDoor()
    {
        if (animator != null)
        {
            animator.SetTrigger("Toggle");
        }
    }
}