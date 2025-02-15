using UnityEngine;

[RequireComponent(typeof(KeyItem))]
public class KeyInteractionHighlight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isPlayerNear;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            HighlightKey();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            ResetHighlight();
        }
    }

    private void HighlightKey()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.yellow;
        }
    }

    private void ResetHighlight()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}