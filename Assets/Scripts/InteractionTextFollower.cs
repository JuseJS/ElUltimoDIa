using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionTextFollower : MonoBehaviour
{
    public Vector3 TargetPosition { get; private set; }
    private RectTransform rectTransform;
    private TextMeshProUGUI textComponent;

    public void Initialize(Vector3 targetPosition)
    {
        TargetPosition = targetPosition;
        rectTransform = GetComponent<RectTransform>();
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Camera.main != null)
        {
            // Actualizar la posición en pantalla
            Vector3 screenPos = Camera.main.WorldToScreenPoint(TargetPosition);
            
            // Si el objeto está detrás de la cámara, ocultar el texto
            if (screenPos.z < 0)
            {
                textComponent.enabled = false;
                return;
            }

            textComponent.enabled = true;
            rectTransform.position = screenPos;
        }
    }
}