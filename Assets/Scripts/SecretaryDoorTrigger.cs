using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SecretaryDoorTrigger : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private MissionData gameMissions;

    private void Start()
    {
        var collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            if (MissionManager.Instance.CurrentMission == gameMissions.enterSchoolMission)
            {
                MissionManager.Instance.CompleteMission();
                UIManager.Instance.ShowMessage("Has entrado en secretaría");
            }
        }
    }
}