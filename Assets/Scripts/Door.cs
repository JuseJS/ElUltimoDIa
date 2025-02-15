using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, IInteractable
{
    public KeyType requiredKeyType;
    public bool isLocked = true;
    
    [Header("Door Movement")]
    public float openAngle = 90f;
    public float openDuration = 1f;
    public Vector3 rotationAxis = Vector3.up;
    
    private bool isOpen = false;
    private Vector3 originalRotation;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Awake()
    {
        // Guardar la rotación original
        originalRotation = transform.localEulerAngles;
        closedRotation = transform.localRotation;
        openRotation = Quaternion.Euler(originalRotation + (rotationAxis * openAngle));
    }

    public void Interact(PlayerInventory playerInventory)
    {
        if (!isLocked)
        {
            ToggleDoor();
            return;
        }

        if (playerInventory.HasKey(requiredKeyType))
        {
            isLocked = false;
            UIManager.Instance.ShowMessage("¡Has abierto la puerta!");
            ToggleDoor();
        }
        else
        {
            UIManager.Instance.ShowMessage("Necesitas la llave correcta para abrir esta puerta.", true);
        }
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        
        // Detener cualquier animación en curso
        transform.DOKill();
        
        // Rotar la puerta
        if (isOpen)
        {
            transform.DOLocalRotateQuaternion(openRotation, openDuration)
                .SetEase(Ease.InOutQuad);
        }
        else
        {
            transform.DOLocalRotateQuaternion(closedRotation, openDuration)
                .SetEase(Ease.InOutQuad);
        }
    }

    // Opcional: Método para restablecer la puerta a su estado original
    public void ResetDoor()
    {
        isOpen = false;
        isLocked = true;
        transform.localRotation = closedRotation;
    }
}