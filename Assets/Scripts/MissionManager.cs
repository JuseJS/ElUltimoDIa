using UnityEngine;
using System;

public class MissionManager : MonoBehaviour 
{
    public static MissionManager Instance { get; private set; }

    public event Action<Mission> OnMissionStarted;
    public event Action<Mission> OnMissionCompleted;
    
    [SerializeField] private MissionData gameMissions;
    
    public Mission CurrentMission { get; private set; }

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

    private void Start()
    {
        // Iniciar con la primera misión
        StartMission(gameMissions.findMainKeyMission);
    }

    public void StartMission(Mission mission)
    {
        if (mission == null) return;
        
        CurrentMission = mission;
        OnMissionStarted?.Invoke(CurrentMission);
        UIManager.Instance.UpdateMission(CurrentMission.description);
    }

    public void CompleteMission()
    {
        if (CurrentMission == null) return;

        OnMissionCompleted?.Invoke(CurrentMission);
        UIManager.Instance.ShowMessage(CurrentMission.completionMessage);

        if (CurrentMission.nextMission != null)
        {
            StartMission(CurrentMission.nextMission);
        }
    }
}