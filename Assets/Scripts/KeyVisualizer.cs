using UnityEngine;

[RequireComponent(typeof(KeyItem))]
public class KeyVisualizer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private KeyItem keyItem;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        keyItem = GetComponent<KeyItem>();
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (keyItem.keyData != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = keyItem.keyData.keySprite;
        }
    }

    // Opcional: Hacer que la llave gire o flote
    private void Update()
    {
        transform.Rotate(Vector3.up * 50f * Time.deltaTime);
    }
}