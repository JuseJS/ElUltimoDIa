using UnityEngine;

public class ComputerIndicator : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private float bobHeight = 0.5f;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private MissionData gameMissions;
    
    private Vector3 startPosition;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        startPosition = transform.localPosition;
        meshRenderer = GetComponent<MeshRenderer>();
        
        // Ocultar el indicador al inicio
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
        
        // Suscribirse a los cambios de misión
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.OnMissionStarted += HandleMissionChanged;
            MissionManager.Instance.OnMissionCompleted += HandleMissionCompleted;
        }
    }

    private void OnDestroy()
    {
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.OnMissionStarted -= HandleMissionChanged;
            MissionManager.Instance.OnMissionCompleted -= HandleMissionCompleted;
        }
    }

    private void Update()
    {
        if (meshRenderer != null && meshRenderer.enabled)
        {
            float newY = startPosition.y + (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
            transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
            
            if (Camera.main != null)
            {
                transform.rotation = Camera.main.transform.rotation;
            }
        }
    }

    private void HandleMissionChanged(Mission mission)
    {
        if (meshRenderer != null && gameMissions != null)
        {
            meshRenderer.enabled = (mission == gameMissions.accessComputerMission);
        }
    }

    private void HandleMissionCompleted(Mission mission)
    {
        if (meshRenderer != null && mission == gameMissions.accessComputerMission)
        {
            meshRenderer.enabled = false;
        }
    }
}